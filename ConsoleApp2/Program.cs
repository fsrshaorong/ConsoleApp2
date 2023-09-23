using System;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleApp2
{
    internal class Class1
    {
        public static void Main()
        {
            string sql = "Data Source=.;Initial Catalog=DBTEST1;Integrated Security=True";
            SqlConnection conn = new SqlConnection(sql);
            conn.Open();
            string str = "select *from People";
            SqlCommand cmd = new SqlCommand(str, conn);
            //sqldatareader对象处理查询的结果
            SqlDataReader reader = cmd.ExecuteReader();
            //处理返回的结果，每次只能读取一条记录
            while(reader.Read())
            {
                //从DataReader对象中获取数据
                string peopleName=reader["PeopleName"].ToString();
                string peopleRender = reader["PeopleRender"].ToString();
                string peopleBirth = reader["PeopleBirth"].ToString();
                Console.WriteLine($"人名：{peopleName},性别：{peopleRender},生日：{peopleBirth}");
            }
            //关闭对象，释放资源 注意顺序
            reader.Close();
            conn.Close();
            Console.ReadLine();
            
        }
        #region 检查用户登录
        public static bool CheckUserLogin(string peopleName,int peopleSalary,ref string strMsg)
        {
            //数据库的连接字符串
            string connstr = @"Data Source=.;Initial Catalog=DBTEST1;Integrated Security=True";
            //创建sqlConnection对象
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                //打开数据库连接对象
                conn.Open();
                //执行的sql语句
                string sql = String.Format("select count(1) from People where PeopleName ='{0}' and PeopleSalary={1}", peopleName, peopleSalary);
                //创建command对象 执行sql命令 并且返回执行结果
                SqlCommand mad = new SqlCommand(sql, conn);
                //执行sql语句 并且返回结果
                int result = (int)mad.ExecuteScalar();
                //根据查询结果 判断用户是否登录成功
                if (result > 0)
                {
                    strMsg = "登陆成功";
                    return true;
                }
                else
                {
                    strMsg = "失败";
                    return false;
                }
            }
            catch(Exception e)
            {
                strMsg = "出现异常";
                Console.WriteLine(e);
            }
            finally
            {
                conn.Close();
            }
            return false;
           
        }
        #endregion

        #region 新增图书信息
        public static bool SavePeople()
        {
           
                Console.WriteLine("请输入人名");
                string departmentName = Console.ReadLine();
                Console.WriteLine("请输入工资");
                string departmentRemark = Console.ReadLine();
                string str = String.Format("insert into Department(DepartmentRemark,DepartmentName) values('{0}','{1}')", departmentRemark, departmentName);
               int result=CommMethod(str);
            if(result>0)
            {
                Console.WriteLine("添加成功");
                return true;
            }
            else
            {
                Console.WriteLine("添加失败");
                return false;
            }
            
        }
        #endregion

        #region 增删改公共方法
        public static int CommMethod(string constr)
        {
            int result = 0;
            string str = @"Data Source=.;Initial Catalog=DBTEST1;Integrated Security=True";
            SqlConnection conn = new SqlConnection(str);
            try
            {
                conn.Open();
                SqlCommand mand = new SqlCommand(constr, conn);
                result = mand.ExecuteNonQuery();
               
            }
            catch (Exception e)
            {
                Console.WriteLine("出现异常");
                Console.WriteLine(e);
                return result;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        #endregion

        

    }

}
