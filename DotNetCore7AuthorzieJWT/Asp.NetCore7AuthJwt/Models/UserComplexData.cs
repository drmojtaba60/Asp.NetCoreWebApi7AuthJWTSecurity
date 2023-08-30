namespace Asp.NetCore7AuthJwt.Models
{
    public class UserComplexData
    {
        public string Identifier { get; set; }
        public DateTime RequestedOn { get; set; }
        public string ClientId { get; set; }
        public UserInfo UserInfo { get; set; }  

        public override string ToString()
        {
            var result = System.Text.Json.JsonSerializer.Serialize(this);
            return result;
            //return result.EncryptAes256(Constants.Keies.Encrypt);
        }
        public static UserComplexData GetInstance(string serializedData)
        {
            var result = new UserComplexData();
            try
            {
               // var ssoDataRequestAsString = serializedData.DecryptAes256(Constants.Keies.Encrypt);
                result = System.Text.Json.JsonSerializer.Deserialize<UserComplexData>(serializedData);
            }
            catch
            {
                result = null;
            }
            return result;

        }
    }
}
