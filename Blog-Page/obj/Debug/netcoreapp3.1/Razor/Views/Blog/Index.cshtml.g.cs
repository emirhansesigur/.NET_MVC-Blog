#pragma checksum "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4b425dc99e044e594e0d116859aba1d05efe33c7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Blog_Index), @"mvc.1.0.view", @"/Views/Blog/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\_ViewImports.cshtml"
using BlogNET;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\_ViewImports.cshtml"
using BlogNET.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4b425dc99e044e594e0d116859aba1d05efe33c7", @"/Views/Blog/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4b2596d011901762c6783f2731878da965fdd644", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Blog_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<Blog>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_UserLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<section class=""main-content-w3layouts-agileits"">
    <div class=""container"">
        <h3 class=""tittle"">Blog Paylaşımları</h3>
        <div class=""inner-sec"">
            <!--left-->
            <div class=""left-blog-info-w3layouts-agileits text-left"">
                <div class=""row"">
");
#nullable restore
#line 14 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
                     foreach (var x in Model)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"col-lg-4 card\">\r\n                            <a");
            BeginWriteAttribute("href", " href=\"", 561, "\"", 587, 2);
            WriteAttributeValue("", 568, "/Blog/Detials/", 568, 14, true);
#nullable restore
#line 17 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
WriteAttributeValue("", 582, x.Id, 582, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                <img");
            BeginWriteAttribute("src", " src=\"", 627, "\"", 645, 1);
#nullable restore
#line 18 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
WriteAttributeValue("", 633, x.ImagePath, 633, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" class=""card-img-top img-fluid"" alt=""Blog Fotoğrafı"">
                            </a>
                            <div class=""card-body"">
                                <ul class=""blog-icons my-4"">
                                    <li>
                                        <a href=""#"">
                                            <i class=""far fa-calendar-alt""></i> ");
#nullable restore
#line 24 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
                                                                           Write(x.CreateTime.ToShortDateString());

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </a>
                                    </li>
                                    <li class=""mx-2"">
                                        <a href=""#"">
                                            <i class=""far fa-comment""></i> 21
                                        </a>
                                    </li>
");
            WriteLiteral("\r\n                                </ul>\r\n                                <h5 class=\"card-title\">\r\n                                    <a");
            BeginWriteAttribute("href", " href=\"", 1829, "\"", 1855, 2);
            WriteAttributeValue("", 1836, "/Blog/Detials/", 1836, 14, true);
#nullable restore
#line 40 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
WriteAttributeValue("", 1850, x.Id, 1850, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 40 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
                                                             Write(x.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                                </h5>\r\n");
#nullable restore
#line 42 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
                                 if (x.Content != null)
                                {
                                    

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <span class=\"card-text mb-3\">");
#nullable restore
#line 45 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
                                                            Write(x.Subtitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 46 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
                                }
                                

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <a");
            BeginWriteAttribute("href", " href=\"", 2397, "\"", 2423, 2);
            WriteAttributeValue("", 2404, "/Blog/Detials/", 2404, 14, true);
#nullable restore
#line 48 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
WriteAttributeValue("", 2418, x.Id, 2418, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-primary read-m\">Okumak İçin Tıklayın</a>\r\n                            </div>\r\n                        </div>\r\n");
#nullable restore
#line 51 "C:\Users\Emir\Desktop\BLOG\Blog-Page\Views\Blog\Index.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </div>\r\n\r\n                <!--//left-->\r\n            </div>\r\n        </div>\r\n    </div>\r\n</section>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<Blog>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
