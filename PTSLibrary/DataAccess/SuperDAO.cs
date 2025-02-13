﻿using PTSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PTSLibrary.DataAccess
{
    public class SuperDAO
    {

        protected UserModel GetUser(int ID)
        {
            string sql;
            SqlConnection con = new(Properties.Settings.Default.PTSConnectionstring);
            SqlCommand cmd;
            SqlDataReader dr;
            UserModel user;

            sql = "SELECT * FROM customer WHERE CustomerID = " + ID;

            cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                dr.Read();
                user = new(dr["FirstName"].ToString(), dr["LastName"].ToString(), (int)dr["ID"]);
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting Customer", ex);
            }
            finally
            {
                con.Close();
            }
            return user;
        }

        //List of the projects
        public List<ProjectModel> GetListOfProjects()
        {
            string sql;
            SqlConnection con = new(Properties.Settings.Default.PTSConnectionstring);
            SqlCommand cmd;
            SqlDataReader dr;
            List<ProjectModel> projects;
            projects = new List<ProjectModel>();
            sql = "SELECT * FROM dbo.Projects";
            cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProjectModel p = new((int)dr["ProjectID"], dr["ProjectCode"].ToString(), dr["ProjectName"].ToString(),
                        dr["ProjectDescription"].ToString(), dr["ProjectTasks"].ToString(), dr["Level"].ToString(),
                        (int)dr["Duration"], dr["GithubRepo"].ToString(), dr["VideoLink"].ToString());
                    projects.Add(p);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting list", ex);
            }
            finally
            {
                con.Close();
            }
            return projects;
        }

        //List the tasks
        public List<TaskModel> GetListOfTasks(int projectID)
        {
            string sql;
            SqlConnection con = new(Properties.Settings.Default.PTSConnectionstring);
            SqlCommand cmd;
            SqlDataReader dr;
            List<TaskModel> tasks;
            tasks = new List<TaskModel>();
            sql = "SELECT * FROM Tasks WHERE ProjectID = '" + projectID + "'";
            cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    TaskModel t = new((int)dr["ID"], dr["Task"].ToString());
                    tasks.Add(t);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error getting task list", ex);
            }
            finally
            {
                con.Close();
            }
            return tasks;
        }

        //List of the completed projects (assigned)
        public List<AssignedProjectModel> GetListOfCompletedProjects(int CohortID)
        {
            string sql;
            SqlConnection con = new(Properties.Settings.Default.PTSConnectionstring);
            SqlCommand cmd;
            SqlDataReader dr;
            List<AssignedProjectModel> completeProjects;
            completeProjects = new List<AssignedProjectModel>();
            sql = "SELECT AssignedProject.*, Projects.ProjectCode, Projects.ProjectName, Projects.Level, Users.FirstName, Users.LastName " +
                "FROM ((AssignedProject INNER JOIN Projects ON AssignedProject.ProjectID = Projects.ProjectID) INNER JOIN Users ON AssignedProject.UserID = Users.ID) " +
                "WHERE Status = 'Complete' AND AssignedProject.CohortID  =" + CohortID + ";";
            cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DateTime startDate = DateTime.Parse(dr["StartDate"].ToString());
                    AssignedProjectModel p = new((int)dr["AssignedID"], startDate, (int)dr["ProjectID"],
                        (int)dr["ProjectID"], (int)dr["UserID"],dr["Status"].ToString(), dr["ProjectCode"].ToString(),
                        dr["ProjectName"].ToString(), dr["Level"].ToString(), dr["FirstName"].ToString(), dr["LastName"].ToString());
                    completeProjects.Add(p);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting list", ex);
            }
            finally
            {
                con.Close();
            }
            return completeProjects;
        }
        //List of the inprogress projects (assigned)
        public List<AssignedProjectModel> GetListOfInprogressProjects(int CohortID)
        {
            string sql;
            SqlConnection con = new(Properties.Settings.Default.PTSConnectionstring);
            SqlCommand cmd;
            SqlDataReader dr;
            List<AssignedProjectModel> inprogressProjects;
            inprogressProjects = new List<AssignedProjectModel>();
            sql = "SELECT AssignedProject.*, Projects.ProjectCode, Projects.ProjectName, Projects.Level, Users.FirstName, Users.LastName " +
                "FROM ((AssignedProject INNER JOIN Projects ON AssignedProject.ProjectID = Projects.ProjectID) INNER JOIN Users ON AssignedProject.UserID = Users.ID) " +
                "WHERE Status = 'Inprogress' AND AssignedProject.CohortID  = " + CohortID + ";";
            cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DateTime startDate = DateTime.Parse(dr["StartDate"].ToString());
                    AssignedProjectModel p = new((int)dr["AssignedID"], startDate, (int)dr["ProjectID"],
                        (int)dr["ProjectID"], (int)dr["UserID"], dr["Status"].ToString(), dr["ProjectCode"].ToString(),
                        dr["ProjectName"].ToString(), dr["Level"].ToString(), dr["FirstName"].ToString(), dr["LastName"].ToString());
                    inprogressProjects.Add(p);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting list", ex);
            }
            finally
            {
                con.Close();
            }
            return inprogressProjects;
        }

    }
}
