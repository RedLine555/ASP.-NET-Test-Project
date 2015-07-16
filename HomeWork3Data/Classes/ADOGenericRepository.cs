﻿using HomeWork3Common.Helpers;
using HomeWork3Common.Helpers.Converter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3Data.Classes
{
    public abstract class ADOGenericRepository<T>: IGenericRepository<T> where T : class, new()
    {
        protected string connectionString;
        protected SqlConnection conn;

        private void initConnection()
        {
            try
            {
                conn = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        public ADOGenericRepository(string connString)
        {
            this.connectionString = connString;
            initConnection();
        }

        private List<String> getTableColumns()
        {
            List<string> colNames = new List<string>();
            bool wasClosed = false;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    wasClosed = true;
                }

                SqlCommand getTableInfo = new SqlCommand(String.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA='dbo'", typeof(T).Name), conn);
                SqlDataReader columns = getTableInfo.ExecuteReader();

                if (columns.HasRows)
                    while (columns.Read())
                        colNames.Add(columns[0] as String);
                columns.Close();
            }
            finally
            {
                if (wasClosed)
                    conn.Close();
            }
            return colNames;
        }

        private List<String> getPrimaryKey()
        {
            List<string> pk = new List<string>();
            bool wasClosed = false;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    wasClosed = true;
                }

                SqlCommand pkQuery = new SqlCommand(String.Format("SELECT Col.Column_Name from\n" +
                                                                   "INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab,\n" +
                                                                   "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col\n" +
                                                                   "WHERE\n" +
                                                                   "Col.Constraint_Name = Tab.Constraint_Name\n" +
                                                                   "AND Col.Table_Name = Tab.Table_Name\n" +
                                                                   "AND Constraint_Type = 'PRIMARY KEY'\n" +
                                                                   "AND Col.Table_Name = '{0}'", typeof(T).Name), conn);

                using (SqlDataReader reader = pkQuery.ExecuteReader())
                {
                    while (reader.Read())
                        pk.Add(reader[0] as String);
                    reader.Close();
                }        
            }
            finally
            {
                if (wasClosed)
                    conn.Close();
            }
            return pk;
        }

        public virtual T FindByID(int id)
        {
            return new T();
        }

        public virtual IQueryable<T> GetAll()
        {
            List<T> allEntities = new List<T>();
            try
            {
                conn.Open();
                SqlCommand getAll = new SqlCommand(String.Format("Select * from [{0}]", typeof(T).Name), conn);
                SqlDataReader result = getAll.ExecuteReader();

                if (result.HasRows)
                {
                    while (result.Read())
                        allEntities.Add(StaticEntitiesConverter.ReaderRowToEntity<T>(result));
                }
            }
            finally
            {
                conn.Close();
            }
            return allEntities.AsQueryable();
        }

        public virtual void Add(T instance)
        {
            try
            {
                conn.Open();

                List<string> colNames = getTableColumns();

                Dictionary<string, object> record = StaticEntitiesConverter.GetProperiesValues<T>(colNames, instance);
                KeyValuePair<string, string> data = StaticQueryHelper.InsertDictionaryToStrings(record);
                SqlCommand insert = new SqlCommand(String.Format("INSERT INTO [{0}] {1} VALUES {2}", typeof(T).Name, data.Key, data.Value), conn);

                foreach(string key in record.Keys)
                if ((record[key] as byte[]) != null)
                    insert.Parameters.Add(String.Format("@{0}", key), SqlDbType.VarBinary, -1).Value = record[key] as byte[];

                insert.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public virtual void Edit(T instance)
        {
            try
            {
                conn.Open();

                string pkName = getPrimaryKey()[0];

                List<string> colNames = getTableColumns();
                Dictionary<string, object> record = StaticEntitiesConverter.GetProperiesValues<T>(colNames, instance);

                String set = StaticQueryHelper.GetModifySetStatementArg(record);
                string pkValS = null;
                Object pkVal = record[pkName];
                if (pkVal.GetType() == typeof(string))
                    pkValS = String.Format("'{0}'", pkVal);
                else
                    pkValS = pkVal.ToString();

                SqlCommand modify = new SqlCommand(String.Format("UPDATE [{0}] SET {1} WHERE {2} = {3}", typeof(T).Name, set, pkName, pkValS), conn);

                foreach (string key in record.Keys)
                    if ((record[key] as byte[]) != null)
                        modify.Parameters.Add(String.Format("@{0}", key), SqlDbType.VarBinary, -1).Value = record[key] as byte[];

                modify.ExecuteNonQuery();

            }
            finally
            {
                conn.Close();
            }
        }

        public virtual void Delete(T instance)
        {
            try
            {
                conn.Open();

                string pkName = getPrimaryKey()[0];
                string pkValS = null;
                Object pkVal = instance.GetType().GetProperty(pkName).GetValue(instance);
                if (pkVal.GetType() == typeof(string))
                    pkValS = String.Format("'{0}'", pkVal);
                else
                    pkValS = pkVal.ToString();

                SqlCommand delete = new SqlCommand(String.Format("DELETE FROM [{0}] WHERE {1} = {2}",typeof(T).Name, pkName, pkValS), conn);
                delete.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public virtual T FindFirst(Func<T, bool> filter)
        {
            IEnumerable<T> result = GetAll();
            return result.Where(filter).FirstOrDefault();

        }

        public virtual IEnumerable<T> FindAll(Func<T, bool> filter)
        {
            IEnumerable<T> result = GetAll();
            return result.Where(filter);
        }
    }
}
