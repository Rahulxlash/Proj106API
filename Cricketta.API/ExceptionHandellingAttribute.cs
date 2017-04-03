using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Cricketta.API
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //if (context.Exception is BusinessException)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //    {
            //        Content = new StringContent(context.Exception.Message),
            //        ReasonPhrase = "Exception"
            //    });

            //}

            ////Log Critical errors
            //Debug.WriteLine(context.Exception);

            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //{
            //    Content = new StringContent("An error occurred, please try again or contact the administrator."),
            //    ReasonPhrase = "Critical Exception"
            //});
            Exception ex = context.Exception;
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CrickContext"].ToString());
            connection.Open();
            SqlCommand cmd = new SqlCommand("Insert into ErrorLog values('" + ex.Message.Substring(0, (ex.Message.Length > 400 ? 399 : ex.Message.Length)) + "',getdate(),'" + ex.StackTrace.Substring(0, (ex.StackTrace.Length > 999 ? 999 : ex.StackTrace.Length)) + "')", connection);
            cmd.ExecuteNonQuery();
            connection.Close();

            //string filePath = @"Error.txt";

            //using (StreamWriter writer = new StreamWriter(filePath, true))
            //{
            //    writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
            //       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
            //    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            //}
        }
    }
}