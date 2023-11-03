using System.Security.Cryptography;
using System.Text;

namespace ServiceToken.tools
{
    public class EncodeID
    {

        public string fn_sha256_Gen()
        {
            Random random = new Random();
            string input = random.Next().ToString();
            using (MD5 sha256 = MD5.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public String fn_sha256Builder(String TextoACifrar)
        {
            String SHA256Hash = "D3AD9315B7BE5DD53B31A273B3B3ABA5DEFE700808305AA16A3062B76658A791"; //equivalente a clave 123
            //SELECT CONVERT(varchar(64), HASHBYTES('SHA2_256', 'demo123'), 2);
            try
            {

                SHA256 sHA256 = SHA256.Create();
                using (SHA256 sha256 = sHA256)
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(TextoACifrar.Trim());
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }

                    SHA256Hash = sb.ToString();

                }

                return SHA256Hash;


            }
            catch (Exception e)
            {
                return SHA256Hash;

            }


            return SHA256Hash;
        }
    }
}