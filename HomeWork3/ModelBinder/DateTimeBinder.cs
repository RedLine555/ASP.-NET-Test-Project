using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork3.ModelBinder
{
    public class DateTimeBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Name == "BirthDay" && propertyDescriptor.PropertyType == typeof(DateTime))
            {
                HttpRequestBase request = controllerContext.HttpContext.Request;
                string day = request.Form.Get("BirthDay.Day");
                string month = request.Form.Get("BirthDay.Month");
                string year = request.Form.Get("BirthDay.Year");
                DateTime birth;
                if (DateTime.TryParse(string.Format("{0}/{1}/{2}",day,month,year),out birth))
                {
                    SetProperty(controllerContext, bindingContext, propertyDescriptor, birth);
                    string s = propertyDescriptor.ToString();
                    return;
                }
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}