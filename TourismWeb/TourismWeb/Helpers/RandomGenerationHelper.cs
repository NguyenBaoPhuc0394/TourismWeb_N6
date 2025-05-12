namespace TourismWeb.Helpers
{
    public class RandomGenerationHelper
    {
        private static readonly Lazy<RandomGenerationHelper> _instance =
            new Lazy<RandomGenerationHelper>(() => new RandomGenerationHelper());

        public static RandomGenerationHelper Instance => _instance.Value;

        private RandomGenerationHelper()
        {
        }

        public string GenerateRandomOtp(int length = 6)
        {
            var random = new Random();
            var otp = "";
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10); 
            }
            return otp;
        }
    }
}
