using System;
using System.Collections.Generic;
using System.Data;
using CompanyComp.Models;
using Microsoft.Data.SqlClient;

namespace CompanyComp
{
    public class App
    {
        public void Run()
        {
            while (true)
            {
                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                    builder.ConnectionString =
                        "Data Source=DESKTOP-LAE0RQ5;Initial Catalog=CompanyComp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    builder.InitialCatalog = "CompanyComp";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        Console.WriteLine(
                            "Type a character between a-f for each of the procedures. Q for quit");
                        string input = Console.ReadLine() ?? string.Empty;
                        switch (input)
                        {
                            case "q":
                                Environment.Exit(0);
                                break;
                            case "a":
                                connection.Open();
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    Console.WriteLine("Department name: ");
                                    string depName = Console.ReadLine() ?? string.Empty;
                                    Console.WriteLine("ManagerSSN (some may be unavailable): \n" +
                                                      "123456789, 333445555, 453453453, 666884444, 888665555, " +
                                                      "987654321, 987987987, 999887777 \nManagerSSN: ");
                                    string mrgSSN = Console.ReadLine() ?? string.Empty;
                                    try
                                    {
                                        cmd.Connection = connection;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "usp_CreateDepartment";
                                        cmd.Parameters.Add("@DName", SqlDbType.VarChar).Value = depName;
                                        cmd.Parameters.Add("@MgrSSN", SqlDbType.Int).Value = mrgSSN;

                                        SqlParameter param = new SqlParameter("@Id", SqlDbType.Int);
                                        param.Direction = ParameterDirection.ReturnValue;
                                        cmd.Parameters.Add(param);
                                        cmd.ExecuteNonQuery();
                                        int id = 0;
                                        if (param.Value != null)
                                        {
                                            id = Convert.ToInt32(param.Value);
                                        }

                                        Console.WriteLine("Department with "+id+ " has been added");
                                    }
                                    catch (SqlException e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                }
                                break;
                            case "b":
                                connection.Open();
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    Console.WriteLine("Department number to update: ");
                                    string depToUpdate = Console.ReadLine() ?? String.Empty;
                                    Console.WriteLine("Update department name to: ");
                                    string updateTo = Console.ReadLine() ?? string.Empty;
                                    try
                                    {
                                        cmd.Connection = connection;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "usp_UpdateDepartmentName";
                                        cmd.Parameters.Add("@DNumber", SqlDbType.Int).Value = depToUpdate;
                                        cmd.Parameters.Add("@DName", SqlDbType.VarChar).Value = updateTo;

                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (SqlException e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                } break;
                            case "c":
                                connection.Open();
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    Console.WriteLine("Department number to update: ");
                                    string depToUpdate = Console.ReadLine() ?? String.Empty;
                                    Console.WriteLine("Update manager SSN to: ");
                                    string updateTo = Console.ReadLine() ?? string.Empty;
                                    try
                                    {
                                        cmd.Connection = connection;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "usp_UpdateDepartmentManager";
                                        cmd.Parameters.Add("@DNumber", SqlDbType.Int).Value = depToUpdate;
                                        cmd.Parameters.Add("@MgrSSN", SqlDbType.Decimal).Value = updateTo;

                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (SqlException e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                } break;
                            case "d":
                                connection.Open();
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    Console.WriteLine("Department number to delete: ");
                                    string depToUpdate = Console.ReadLine() ?? String.Empty;
                                    try
                                    {
                                        cmd.Connection = connection;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "usp_DeleteDepartment";
                                        cmd.Parameters.Add("@DNumber", SqlDbType.Int).Value = depToUpdate;

                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (SqlException e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                } break;
                            case "e":
                                connection.Open();
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    Console.WriteLine("Department number to list: ");
                                    string depToUpdate = Console.ReadLine() ?? String.Empty;
                                    try
                                    {
                                        cmd.Connection = connection;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "usp_GetDepartment";
                                        cmd.Parameters.Add("@DNumber", SqlDbType.Int).Value = depToUpdate;
                                        List<Department> departments = new List<Department>();
                                        var reader = cmd.ExecuteReader();
                                        departments = GetList<Department>(reader);
                                        if (departments!=null)
                                        {
                                            foreach (var department in departments)
                                            {
                                                Console.Write("DNumber: "+ department.DNumber+"\n"+
                                                    "Name: "+ department.DName+ "\n"+
                                                    "MgrSSN: "+department.MgrSSN+"\n"+
                                                    "MgrStartDate: "+department.MgrStartDate+"\n"+ 
                                                    "EmpCount: "+department.EmpCount+"\n");
                                            }
                                        }
                                    }
                                    catch (SqlException e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    finally{connection.Close();}
                                } break;
                            case "f":
                                connection.Open();
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    try
                                    {
                                        cmd.Connection = connection;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "usp_GetAllDepartments";
                                        List<Department> departments = new List<Department>();
                                        var reader = cmd.ExecuteReader();
                                        departments = GetList<Department>(reader);
                                        if (departments!=null)
                                        {
                                            foreach (var department in departments)
                                            {
                                                Console.Write("DNumber: "+ department.DNumber+"\n"+
                                                    "Name: "+ department.DName+ "\n"+
                                                    "MgrSSN: "+department.MgrSSN+"\n"+
                                                    "MgrStartDate: "+department.MgrStartDate+"\n"+ 
                                                    "EmpCount: "+department.EmpCount+"\n"+ "\n");
                                            }
                                        }
                                    }
                                    catch (SqlException e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    finally{connection.Close();}
                                } break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private List<T> GetList<T>(SqlDataReader reader)
        {
            List<T> list = new List<T>();
            while (reader.Read())
            {
                var type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                {
                    var propType = prop.PropertyType;
                    prop.SetValue(obj,Convert.ChangeType(reader[prop.Name].ToString(),propType));
                }
                list.Add(obj);
            }

            return list;
        }
    }
}