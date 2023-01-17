using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CrudDapper.Models;
using Dapper;
using System.Linq;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;

namespace CrudDapper.Repository
{
    public class EmpRepository
    {
        public SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["EmpString"].ConnectionString.ToString();
            con = new SqlConnection(constr);
        }
        //To Add Employee details
        public void AddEmployee(EmpModel objEmp)
        {
            //Additing the employess
            try
            {
                DynamicParameters param = new DynamicParameters();

                param.Add("@Name", objEmp.Name);
                param.Add("@Gender", objEmp.Gender);
                param.Add("@Email", objEmp.Email);
                param.Add("@Language", objEmp.Language);
                param.Add("@Phone", objEmp.Phone);
                param.Add("@Status", 1);
                connection();
                con.Open();
                con.Execute("AddNewEmpDetails", param, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //To view employee details with generic list 
        public List<EmpModel> GetAllEmployees()
        {
            try
            {

                connection();
                con.Open();
                IList<EmpModel> EmpList = SqlMapper.Query<EmpModel>(con, "GetEmployees").ToList();
                con.Close();
                return EmpList.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<EmpModel> GetDeletedEmployees()
        {
            try
            {

                connection();
                con.Open();
                IList<EmpModel> EmpList = SqlMapper.Query<EmpModel>(con, "GetDeletedEmployees").ToList();
                con.Close();
                return EmpList.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //To Update Employee details
        public void UpdateEmployee(EmpModel objUpdate)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpId", objUpdate.EmpId);
                param.Add("@Name", objUpdate.Name);
                param.Add("@Gender", objUpdate.Gender);
                param.Add("@Email", objUpdate.Email);
                param.Add("@Language", objUpdate.Language);
                param.Add("@Phone", objUpdate.Phone);
                param.Add("@Status", 1);
                connection();
                con.Open();
                con.Execute("UpdateEmpDetails", objUpdate, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        //To delete Employee details
        public bool DeleteEmployee(int Id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpId", Id);
                connection();
                con.Open();
                con.Execute("DeleteEmpById", param, commandType: CommandType.StoredProcedure);
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Log error as per your need 
                throw ex;
            }
        }
        public void RestoreDeletedEmployee(int Id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmpId", Id);
                connection();
                con.Open();
                con.Execute("RestoreDeletedEmpById", param, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception ex)
            {
                //Log error as per your need 
                throw ex;
            }
        }

    }
}
