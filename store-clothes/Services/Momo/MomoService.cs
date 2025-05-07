using Microsoft.Extensions.Options;
using store_clothes.Models.Momo;
using store_clothes.Models;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using RestSharp;


namespace store_clothes.Services.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfo model)
        {
            if (_options.Value == null || string.IsNullOrEmpty(_options.Value.SecretKey))
            {
                throw new Exception("Thông tin cấu hình MoMo bị thiếu!");
            }

            if (model == null || model.Amount <= 0)
            {
                throw new ArgumentException("Thông tin đơn hàng không hợp lệ!");
            }

            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfomation = $"Khách hàng: {model.FullName}. Nội dung: {model.OrderInfomation}";

            // ✅ Chuyển amount về kiểu số nguyên (MoMo yêu cầu int)
            var rawData =
              $"partnerCode={_options.Value.PartnerCode}" +
              $"&accessKey={_options.Value.AccessKey}" +
              $"&requestId={model.OrderId}" +
              $"&amount={model.Amount}" +
              $"&orderId={model.OrderId}" +
              $"&orderInfo={model.OrderInfomation}" +
              $"&returnUrl={_options.Value.ReturnUrl}" +
              $"&notifyUrl={_options.Value.NotifyUrl}" +
              $"&extraData=";

            // ✅ Tạo chữ ký chính xác
            string signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);
            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");


            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString(),
                orderInfo = model.OrderInfomation,
                requestId = model.OrderId,
                extraData = "",
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
            return momoResponse;

        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo

            };

        }

        // ✅ Sửa lỗi encoding khi tính HMAC SHA-256
        private string ComputeHmacSha256(string rawData, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(rawData);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

    }
}

