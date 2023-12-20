using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Infrastructure.Tests.PlaywrightTests;

[TestFixture]
public class LoginPage : PageTest
{
    
    [SetUp]
    public void Setup()
    {
        Helper.CreateAndDeleteAdminUser(create: true, delete: false);
    }

    [TearDown]
    public void Teardown()
    {
        Helper.CreateAndDeleteAdminUser(create: false, delete: true);
    }
    
    
    [Test]
    public async Task Loginpage()
    {
        await Page.GotoAsync("http://localhost:5000/login");
       
        var loginFormTitle =  Page.Locator("text=Sign in to Learning Platform");
        await Expect(loginFormTitle).ToBeVisibleAsync();
        

        var emailField =  Page.Locator(".input-wrapper").First;
        await Expect(emailField).ToBeVisibleAsync();
        
        var passwordField =  Page.Locator("ion-item:nth-child(3) > .item-native > .item-inner > .input-wrapper");
        await Expect(passwordField).ToBeVisibleAsync();
    }

    /*
    [Test]
    public async Task Adminlogin()
    {
        await Page.GotoAsync("http://localhost:5000/login");
        
        await Page.GetByLabel("Username or email address").ClickAsync();

        await Page.GetByLabel("Username or email address").FillAsync("test@admin.com");

        await Page.Locator("ion-item:nth-child(3) > .item-native > .item-inner > .input-wrapper").ClickAsync();

        await Page.GetByLabel("Password").FillAsync("TTTTTTTT");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));

    }
    */
    

    [Test]
    public async Task fromLoginToRegister()
    {
        await Page.GotoAsync("http://localhost:5000/login");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Create an account" }).ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/register"));
    }

    [Test]
    public async Task forgotPassword()
    {
        await Page.GotoAsync("http://localhost:5000/login");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Forgot Password?" }).ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/request-reset-password"));
    }
    
}