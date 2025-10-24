using System.Data.SqlClient;
using Contract_Monthly_System.Models.Services;

namespace Contract_Monthly_System.Models
{
    public class Database
    {

        private string connection = @"Server=(localdb)\Contract_Monthly_System;Database=Contract_monthly_system;Trusted_Connection=True;";



        //method to create a table for the database 
        public void creates_table()
        {

            //try and catch for error handling 
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {

                    //open the connection 
                    connect.Open();
                    //string to create a table
                    string query = @"
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Lecturers')
BEGIN
    CREATE TABLE Lecturers (
        LecturerID INT IDENTITY(1,1) PRIMARY KEY,
        LecturerName VARCHAR(100) NOT NULL,
        Email VARCHAR(100),
        Department VARCHAR(100)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserID INT IDENTITY(1,1) PRIMARY KEY,
        UserFullName VARCHAR(100),
        Email VARCHAR(100),
        Password VARCHAR(100),
        Role VARCHAR(50)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Claims')
BEGIN
    CREATE TABLE Claims (
        ClaimID INT IDENTITY(1,1) PRIMARY KEY,
        LecturerID INT FOREIGN KEY REFERENCES Lecturers(LecturerID),
        Month VARCHAR(20) NOT NULL,
        Module VARCHAR(100) NOT NULL,
        HoursWorked FLOAT NOT NULL,
        HourlyRate FLOAT NOT NULL,
        Notes VARCHAR(255),
        State VARCHAR(20) DEFAULT 'Draft',
        SubmittedOn DATETIME DEFAULT GETUTCDATE(),
        Total AS (HoursWorked * HourlyRate) PERSISTED,
);
END
";


                    //use the following to command queries
                    using (SqlCommand create = new SqlCommand(query, connect))
                    {
                        //execute the query
                        create.ExecuteNonQuery();
                        //display message for table being succesfully created 
                        Console.WriteLine("Table was successfully created");


                    }
                    connect.Close();
                }
            }
            catch (Exception error)
            {
                //Error message
                Console.WriteLine(error.Message);



            }

        }



        public bool store_user(string fullName, string email, string password, string role)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    // 🔹 Insert into Users
                    string userQuery = @"INSERT INTO Users (UserFullName, Email, Password, Role)
                                 VALUES (@fullName, @Email, @Password, @Role)";
                    using (SqlCommand userCmd = new SqlCommand(userQuery, connect))
                    {
                        userCmd.Parameters.AddWithValue("@fullName", fullName);
                        userCmd.Parameters.AddWithValue("@Email", email);
                        userCmd.Parameters.AddWithValue("@Password", password); // consider hashing
                        userCmd.Parameters.AddWithValue("@Role", role);
                        userCmd.ExecuteNonQuery();
                    }

                    // 🔹 If role is Lecturer, insert into Lecturers
                    if (role == "Lecturer")
                    {
                        string lecturerQuery = @"INSERT INTO Lecturers (LecturerName)
                                         SELECT @fullName
                                         WHERE NOT EXISTS (
                                             SELECT 1 FROM Lecturers WHERE LecturerName = @fullName
                                         )";
                        using (SqlCommand lecturerCmd = new SqlCommand(lecturerQuery, connect))
                        {
                            lecturerCmd.Parameters.AddWithValue("@fullName", fullName);
                            lecturerCmd.ExecuteNonQuery();
                        }
                    }
                }

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine("Error inserting user or lecturer: " + error.Message);
                return false;
            }
        }



        public User? get_user(string fullName, string password)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();
                    string query = @"SELECT * FROM Users WHERE UserFullName = @fullName AND Password = @password";

                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.Parameters.AddWithValue("@fullName", fullName);
                        command.Parameters.AddWithValue("@password", password);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserFullName = reader["UserFullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    Password = reader["Password"].ToString()
                                };
                            }
                        }
                    }

                    connect.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            return null;
        }


        public bool store_claim(LectureViewer claim)
        {
            Console.WriteLine("store_claim() called");

            claim.ClaimState = ClaimState.Pending;

            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    // 🔹 Step 1: Use LecturerID already passed in
                    int lecturerId = claim.LecturerID;

                    // 🔹 Step 2: Insert claim
                    string query = @"INSERT INTO Claims (LecturerID, Month, Module, HoursWorked, HourlyRate, Notes, State, SupportingDocumentPath, TotalAmount)
                             VALUES (@LecturerID, @Month, @Module, @HoursWorked, @HourlyRate, @Notes, @State, @SupportingDocumentPath, @TotalAmount)";

                    SqlCommand cmd = new SqlCommand(query, connect);

                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    cmd.Parameters.AddWithValue("@Month", claim.Month);
                    cmd.Parameters.AddWithValue("@Module", claim.Module);
                    cmd.Parameters.AddWithValue("@HoursWorked", claim.HoursWorked);
                    cmd.Parameters.AddWithValue("@HourlyRate", claim.HourlyRate);
                    cmd.Parameters.AddWithValue("@Notes", claim.Notes);
                    cmd.Parameters.AddWithValue("@State", claim.ClaimState.ToString());
                    cmd.Parameters.AddWithValue("@SupportingDocumentPath", claim.SupportingDocumentPath ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TotalAmount", claim.HoursWorked * claim.HourlyRate); // ✅ Inline calculation
                    Console.WriteLine("Executing claim insert for LecturerID: " + lecturerId);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Claim insert failed: " + ex.Message, ex);
            }


        }






        public List<LectureViewer> get_all_claims()
        {
            List<LectureViewer> claims = new();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                string query = @"SELECT L.LecturerName, C.Month, C.Module, C.HoursWorked, C.HourlyRate, C.Notes
                         FROM Claims C
                         JOIN Lecturers L ON C.LecturerID = L.LecturerID";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        claims.Add(new LectureViewer
                        {
                            LecturerName = reader["LecturerName"].ToString(),
                            Month = reader["Month"].ToString(),
                            Module = reader["Module"].ToString(),
                            HoursWorked = Convert.ToDouble(reader["HoursWorked"]),
                            HourlyRate = Convert.ToDouble(reader["HourlyRate"]),
                            Notes = reader["Notes"].ToString()
                        });
                    }
                }

                connect.Close();
            }

            return claims;

        }
        public bool update_claim_status(int claimId, string newStatus)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();
                    string query = "UPDATE Claims SET State = @State WHERE ClaimID = @ClaimID";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@State", newStatus);
                    cmd.Parameters.AddWithValue("@ClaimID", claimId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // ✅ true if update succeeded
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating claim status: " + ex.Message);
                return false; // ✅ failure fallback
            }
        }


        public List<LectureViewer> get_pending_claims()
        {
            List<LectureViewer> claims = new();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                string query = @"SELECT C.ClaimID, L.LecturerName, C.Month, C.Module, C.HoursWorked, C.HourlyRate, C.Notes, C.State, C.SupportingDocumentPath
FROM Claims C
JOIN Lecturers L ON C.LecturerID = L.LecturerID
WHERE C.State = 'Draft' OR C.State = 'Pending'";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        claims.Add(new LectureViewer
                        {
                            ClaimID = Convert.ToInt32(reader["ClaimID"]),
                            LecturerName = reader["LecturerName"].ToString(),
                            Month = reader["Month"].ToString(),
                            Module = reader["Module"].ToString(),
                            HoursWorked = Convert.ToDouble(reader["HoursWorked"]),
                            HourlyRate = Convert.ToDouble(reader["HourlyRate"]),
                            Notes = reader["Notes"].ToString(),
                            SupportingDocumentPath = reader["SupportingDocumentPath"].ToString(),
                            ClaimState = Enum.Parse<ClaimState>(reader["State"].ToString())

                        });
                    }
                }

                connect.Close();
            }

            return claims;
        }

        public bool UpdateClaimState(int claimId, string newState)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();
                    string query = "UPDATE Claims SET State = @State WHERE ClaimID = @ClaimID";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@State", newState);
                    cmd.Parameters.AddWithValue("@ClaimID", claimId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // ✅ true if update succeeded
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating claim state: " + ex.Message);
                return false; // ✅ failure fallback
            }
        }


        public List<Claim> GetClaimsByStates(string[] states)
        {
            List<Claim> claims = new();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();

                // Build parameterized IN clause
                var paramNames = states.Select((s, i) => $"@State{i}").ToArray();
                string inClause = string.Join(",", paramNames);

                string query = $@"
           SELECT C.ClaimID, L.LecturerName, C.Month, C.Module, C.HoursWorked, C.HourlyRate, C.Notes, C.State, C.SupportingDocumentPath
            FROM Claims c
            JOIN Lecturers l ON c.LecturerID = l.LecturerID
            WHERE c.State IN ({inClause})";

                SqlCommand cmd = new SqlCommand(query, connect);

                for (int i = 0; i < states.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramNames[i], states[i]);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        claims.Add(new Claim
                        {
                            ClaimId = Convert.ToInt32(reader["ClaimID"]),
                            LecturerName = reader["LecturerName"].ToString(),
                            Module = reader["Module"].ToString(),
                            Month = reader["Month"].ToString(),
                            HoursWorked = Convert.ToDouble(reader["HoursWorked"]),
                            HourlyRate = Convert.ToDouble(reader["HourlyRate"]),
                            Notes = reader["Notes"].ToString(),
                            State = Enum.Parse<ClaimState>(reader["State"].ToString()),
                            SupportingDocumentPath = reader["SupportingDocumentPath"].ToString() // ✅ Add this line
                        });

                    }
                }
            }

            return claims;
        }


        public List<LectureViewer> GetClaimsByLecturerId(int lecturerId)
        {
            var claims = new List<LectureViewer>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                string query = "SELECT * FROM Claims WHERE LecturerID = @LecturerID ORDER BY SubmittedOn DESC";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            claims.Add(new LectureViewer
                            {
                                ClaimID = reader["ClaimID"] != DBNull.Value ? Convert.ToInt32(reader["ClaimID"]) : 0,
                                LecturerID = reader["LecturerID"] != DBNull.Value ? Convert.ToInt32(reader["LecturerID"]) : 0,
                                Month = reader["Month"]?.ToString() ?? "",
                                Module = reader["Module"]?.ToString() ?? "",
                                HoursWorked = reader["HoursWorked"] != DBNull.Value ? Convert.ToDouble(reader["HoursWorked"]) : 0,
                                HourlyRate = reader["HourlyRate"] != DBNull.Value ? Convert.ToDouble(reader["HourlyRate"]) : 0,
                                Notes = reader["Notes"]?.ToString() ?? "",
                                ClaimState = reader["State"] != DBNull.Value
                                    ? Enum.TryParse<ClaimState>(reader["State"].ToString(), out var state) ? state : ClaimState.Pending
                                    : ClaimState.Pending,
                                SupportingDocumentPath = reader["SupportingDocumentPath"]?.ToString() ?? "",
                                Total = reader["TotalAmount"] != DBNull.Value ? Convert.ToDouble(reader["TotalAmount"]) : 0
                            });
                        }
                    }
                }
            }

            return claims;
        }

    }
}