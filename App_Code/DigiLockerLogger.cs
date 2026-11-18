using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

public static class DigiLockerLogger
{
    public class Entry
    {
        public long FormNo;
        public string Step;
        public bool IsSuccess;
        public string SessionId, TransactionId;
        public int? HttpStatus, DurationMs;
        public string AadhaarMasked, AadhaarHash, Name, Dob, Gender;
        public string Address, City, District, State, Pincode;
        public string Message, RequestJson, ResponseJson;
        public string Ip, PageName;
    }

    private static object N(string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return DBNull.Value;
        return s.Length > max ? s.Substring(0, max) : s;
    }

    /// <summary>Kabhi exception nahi phenkta: log fail hone se KYC nahi rukega.</summary>
    public static void Save(string connectionString, Entry e)
    {
        try
        {
            const string sql =
                "INSERT INTO DigiLockerLog (FormNo, Step, IsSuccess, SessionId, TransactionId, HttpStatus, DurationMs, " +
                "AadhaarMasked, AadhaarHash, NameAsPerAadhaar, Dob, Gender, Address, City, District, State, Pincode, " +
                "Message, RequestJson, ResponseJson, IpAddress, PageName) " +
                "VALUES (@FormNo, @Step, @IsSuccess, @SessionId, @TransactionId, @HttpStatus, @DurationMs, " +
                "@AadhaarMasked, @AadhaarHash, @Name, @Dob, @Gender, @Address, @City, @District, @State, @Pincode, " +
                "@Message, @Req, @Res, @Ip, @Page)";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@FormNo", SqlDbType.Decimal).Value = e.FormNo;
                cmd.Parameters.Add("@Step", SqlDbType.VarChar, 30).Value = e.Step;
                cmd.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = e.IsSuccess;
                cmd.Parameters.AddWithValue("@SessionId", N(e.SessionId, 100));
                cmd.Parameters.AddWithValue("@TransactionId", N(e.TransactionId, 100));
                cmd.Parameters.AddWithValue("@HttpStatus", (object)e.HttpStatus ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DurationMs", (object)e.DurationMs ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AadhaarMasked", N(e.AadhaarMasked, 20));
                cmd.Parameters.AddWithValue("@AadhaarHash", N(e.AadhaarHash, 128));
                cmd.Parameters.AddWithValue("@Name", N(e.Name, 150));
                cmd.Parameters.AddWithValue("@Dob", N(e.Dob, 20));
                cmd.Parameters.AddWithValue("@Gender", N(e.Gender, 10));
                cmd.Parameters.AddWithValue("@Address", N(e.Address, 500));
                cmd.Parameters.AddWithValue("@City", N(e.City, 100));
                cmd.Parameters.AddWithValue("@District", N(e.District, 100));
                cmd.Parameters.AddWithValue("@State", N(e.State, 100));
                cmd.Parameters.AddWithValue("@Pincode", N(e.Pincode, 10));
                cmd.Parameters.AddWithValue("@Message", N(e.Message, 500));
                cmd.Parameters.AddWithValue("@Req", e.RequestJson == null ? (object)DBNull.Value : e.RequestJson);
                cmd.Parameters.AddWithValue("@Res", e.ResponseJson == null ? (object)DBNull.Value : e.ResponseJson);
                cmd.Parameters.AddWithValue("@Ip", N(e.Ip, 50));
                cmd.Parameters.AddWithValue("@Page", N(e.PageName, 50));

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Trace.TraceError("DigiLockerLogger failed: " + ex.Message);
        }
    }
}