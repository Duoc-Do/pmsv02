using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WebApp.Areas.Accounting.Services
{


    public sealed class Crypto
    {
        /// <summary>
        /// RSA private key
        /// </summary>
        private const string PRIVATE_KEY = "<RSAKeyValue><Modulus>8scg92DxAK5gd/6ntZ+MdHWEgsv6y5wIoPFAhhtxhEMka8iD+RvY9MDZQ65EGKep+KV6OcN7sYi9oRGoQib8Q/7TTUc7a5zidPrplaYrIGxI3ns3E71kgLk/U8As1Kmb/zqKzdVv2XVkaf1OBSoVUdpa/2wAwyb13J/h2f5glik=</Modulus><Exponent>AQAB</Exponent><P>/qGusp90O+OqIhHFt4koiwGgPKYnyU+9qa802lI4i6WVG1GvXZOd5SA9DS6GQYCHL+ImYLvjRyr77bI9kW9/qw==</P><Q>9BUjYjfUPMirbbvvKy2MQ2tD29Un/y4hocMDWz5pummZ4O58S506Rlcm4InmJ3dd9pc07bLjAbjER9kIjKe9ew==</Q><DP>PRqxoJ3RN9n/ZuOa7dtVRl5ihIte+tlO61xnM4kNlr1qlb65dZxKBMUCwZoLj8Z3Ko97pDUSam6vPDBMxAzctw==</DP><DQ>8Lc8GyxGIuAguOskV2fnMcJCvTX96RsczgSedckayl5FBOEOMiBQjXh7/evh0MBXKc87wsSuPk9zPAxd8yGmqw==</DQ><InverseQ>dvxVY7ZoQD6kcPtdYmB6kknN49WkQeSyjS8HA/Z8GMbjJiL5NspcMB1Bm44ojKy31VcK0J/CtjBVQzduBkzsOw==</InverseQ><D>4sVd5bSHaTt4oJ0dymjdqWqb/BDMkqOM68htnNbFkeWlf4gsveNlJDl+t9lvghgajEcFhC80uwAfIPR9Vk1UE918PRc0sh6A75f80lW/yUYK1DalbMS1KNSHYdJ3VBnozhthW1o4/9stqxujgKvh8EeV3NZCJxDHmgM6G6bE3IE=</D></RSAKeyValue>";

        /// <summary>
        /// RSA public key
        /// </summary>
        private const string PUBLIC_KEY = "<RSAKeyValue><Modulus>8scg92DxAK5gd/6ntZ+MdHWEgsv6y5wIoPFAhhtxhEMka8iD+RvY9MDZQ65EGKep+KV6OcN7sYi9oRGoQib8Q/7TTUc7a5zidPrplaYrIGxI3ns3E71kgLk/U8As1Kmb/zqKzdVv2XVkaf1OBSoVUdpa/2wAwyb13J/h2f5glik=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// Hide from constructing a new instance
        /// </summary>
        private Crypto()
        {

        }

        /// <summary>
        /// Encrypt supplying data by RSA algorithm
        /// </summary>
        /// <param name="data">Encrypting data</param>
        /// <returns>Encrypted data</returns>
        public static string Encrypt(string data)
        {
            // RSA
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PRIVATE_KEY);

            string[] parts = Split32(data);
            string[] results = new string[parts.Length];

            // Encrypt parts of maximum 100 characters
            for (int i = 0; i < parts.Length; i++)
            {
                byte[] buf = UnicodeEncoding.Unicode.GetBytes(parts[i]);

                // Encrypt
                buf = rsa.Encrypt(buf, false);

                results[i] = Convert.ToBase64String(buf);
            }

            return string.Join(":", results);
        }

        /// <summary>
        /// Decrypt data has encrypted by RSA algorithm
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns>Original data</returns>
        public static string Decrypt(string encryptedData)
        {
            // RSA
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PRIVATE_KEY);

            string[] parts = encryptedData.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            string[] data = new string[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                byte[] buf = Convert.FromBase64String(parts[i]);

                // Decrypt
                buf = rsa.Decrypt(buf, false);

                data[i] = UnicodeEncoding.Unicode.GetString(buf);
            }

            return string.Join("", data);
        }

        /// <summary>
        /// Split a string into strings of 32 characters maximum
        /// </summary>
        /// <param name="data">String to split</param>
        /// <returns>Array of strings of 32 characters maximum</returns>
        private static string[] Split32(string data)
        {
            int len = data.Length;
            int p = (len / 32) + (len % 32 > 0 ? 1 : 0);
            string[] parts = new string[p];

            for (int i = 0; i < p; i++)
            {
                int z = i * 32;

                parts[i] = data.Substring(z, (z + 32 > len) ? (len - z) : 32);
            }

            return parts;
        }
    }

    public class Users
    {
        private int _UserID;
        private int _RoleID;
        private string _Name;
        private string _FullName;
        private bool _IsAdmin;
        private bool _IsLogin;
        private bool _IsLicence;

        public int RoleID { get { return this._RoleID; } set { this._RoleID = value; } }

        public string Name { get { return this._Name; } set { this._Name = value; } }

        public string FullName { get { return this._FullName; } set { this._FullName = value; } }

        public int UId { get { return this._UserID; } set { this._UserID = value; } }

        public bool IsLogin { get { return this._IsLogin; } set { this._IsLogin = value; } }

        public bool IsAdmin { get { return this._IsAdmin; } set { this._IsAdmin = value; } }

        public bool IsLicence { get { return this._IsLicence; } set { this._IsLicence = value; } }

        public bool Login(string strPassword)
        {
            //LotusLib.Data.SqlClient.SqlConnectionObject.AppCnn.Open();

            //LotusLib.Data.SqlClient.DisconnectedObject DataObject = new LotusLib.Data.SqlClient.DisconnectedObject();

            //DataObject.Cmd.CommandText = "SELECT SysUser.UserID, SysUser.Name, SysUser.Password, SysUser.FullName, SysUser.RoleID, SysUser.CreatedBy, SysUser.CreatedDateTime, SysUser.ModifiedBy, SysUser.ModifiedDateTime, SysRole.IsAdmin FROM SysUser INNER JOIN SysRole ON SysUser.RoleID = SysRole.RoleID WHERE     (SysUser.Password = @Password)";

            //DataObject.Cmd.CommandType = CommandType.Text;
            //DataObject.Cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Password", strPassword));

            bool _ret = false;


            //try
            //{
            //    DataObject.Dr = DataObject.Cmd.ExecuteReader();
            //    _ret = DataObject.Dr.HasRows;
            //    if (_ret)
            //    {
            //        DataObject.Dr.Read();
            //        this._UserID = (int)DataObject.Dr["UserID"];
            //        this._Name = DataObject.Dr["Name"].ToString();
            //        this._FullName = DataObject.Dr["FullName"].ToString();
            //        this._RoleID = (int)DataObject.Dr["RoleID"];
            //        this._IsAdmin = (bool)DataObject.Dr["IsAdmin"];

            //    }
            //    this._IsLogin = _ret;
            //}
            //catch (SystemException)
            //{
            //    LotusLib.Forms.MessageBox.Show("Cấu hình không đúng.\n Xin vui lòng kiểm tra lại cấu hình server!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //}

            //LotusLib.Data.SqlClient.SqlConnectionObject.AppCnn.Close();

            //if (_ret)
            //{
            //    LotusLib.SolutionsDataSetTableAdapters.SysBusinessRoleTableAdapter daBusinessRole = new LotusLib.SolutionsDataSetTableAdapters.SysBusinessRoleTableAdapter();
            //    daBusinessRole.Connection = LotusLib.Data.SqlClient.SqlConnectionObject.AppCnn;
            //    daBusinessRole.Fill(Security.BusinessRole.tbBusinessRole, LotusLib.Security.User.objUser.RoleID);

            //    LotusLib.SolutionsDataSetTableAdapters.SysMessageTableAdapter daSysMessage = new LotusLib.SolutionsDataSetTableAdapters.SysMessageTableAdapter();
            //    daSysMessage.Connection = LotusLib.Data.SqlClient.SqlConnectionObject.AppCnn;
            //    daSysMessage.Fill(Utils.SysMessage.tbSysMessage);

            //    #region Khởi tạo biến hệ thống
            //    LotusLib.Sys.GlobeVariant.InitVariant();
            //    #endregion

            //    this._IsLicence = SerialKey.CheckSerialKey();
            //    if (!this.IsLicence)
            //    {
            //        LotusLib.Sys.GlobeVariant.SysOption["LockDate"] = DateTime.Parse("31/12/9998");
            //    }

            //}
            return _ret;

        }

        //public GenericPrincipal GetLogin(string strDatabaseName, string strPassword)
        //{

        //    // Create the Generic Identity representing the User

        //    GenericIdentity GenIdentity = new GenericIdentity(strDatabaseName);

        //    // Define the role membership an array

        //    string[] Roles = { "" };

        //    GenericPrincipal GenPrincipal = new GenericPrincipal(GenIdentity, Roles);
        //    return GenPrincipal;

        //}

        //public bool IsAdministrator()
        //{
        //    // Procedure checks if the Windows Login is an Administrator
        //    // For single role-based validation
        //    // WinPrincipal new WindowsPrincipal(WindowsIdentity.GetCurrent())
        //    // For repeated role-based validation

        //    AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

        //    WindowsPrincipal WinPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;

        //    // Check if the user account is an Administrator

        //    if (WinPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
        //    {

        //        return true;
        //    }
        //    else
        //    {

        //        return false;

        //    }

        //}

    }

    public static class User
    {
        //string usercachekey = "AppAccountingUserKey";
        public static bool Login(string username, string password) 
        {

            string strPassword = Accounting.Services.MD5.GenerateHashDigest(username + password);


            Accounting.Models.WebAppAccEntities db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            
            var user = db.SysUserViews.Where(m => m.Name == username && m.Password == strPassword).FirstOrDefault();
            if (user!=null)
            {
                //lưu user vào cache
                GlobalVariant.SetAppUser(user);
                return true;
            }

            return false;
        }

        public static Users objUser = new Users();


    }

    public static class MD5
    {
        public static string GenerateHashDigest(string strSource)
        {
            byte[] hash;
            // Create an Encoding object so that you can use the convenient GetBytes 
            // method to obtain byte arrays.
            UnicodeEncoding uEncode = new UnicodeEncoding();
            // Create a byte array from the source text passed an argument.
            byte[] bytProducts = uEncode.GetBytes(strSource);
            // The code is almost identical for all three hash types.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            hash = md5.ComputeHash(bytProducts);
            // Base64 is a method of encoding binary data ASCII text.
            return Convert.ToBase64String(hash);

        }
    }

    public static class EncryptedData
    {
        public static string Encrypt(string strSource)
        {
            return Crypto.Encrypt(strSource);
        }
        public static string Decrypt(string strSource)
        {
            return Crypto.Decrypt(strSource);
        }
    }

    public class Security
    {
    }
}