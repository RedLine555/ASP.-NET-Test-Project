using System.Web;
using System.Web.Mvc;

public static class MyExtention
{
public static HtmlString MailLink (this HtmlHelper arr, string mail, string title )
{
    if (mail == string.Empty) { return new HtmlString(""); }
    else return new HtmlString(string.Format("<a href=\"mailto:({1})\">{1}</a>", mail, title));
}
}