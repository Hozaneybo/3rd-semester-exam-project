using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Infrastructure.Tests.PlaywrightTests;

[TestFixture]
public class RegisterPage : PageTest
{
    
    [TearDown]
    public void Teardown()
    {
        Helper.DeleteUserByEmail("playwriht@playwright.com");
    }
    
    [Test]
    public async Task SignUp()
    {
        await Page.GotoAsync("http://localhost:5000/register", new PageGotoOptions { Timeout = 60000 });
        
        Page.SetDefaultTimeout(10000);
        
        await Page.GetByLabel("Full Name").ClickAsync();

        await Page.GetByLabel("Full Name").FillAsync("Play Wright Test ");

        await Page.GetByLabel("Full Name").PressAsync("Tab");

        await Page.GetByLabel("Email", new() { Exact = true }).FillAsync("playwriht@playwright.com");

        await Page.GetByLabel("Email", new() { Exact = true }).PressAsync("Tab");

        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync("pppppppp");

        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).PressAsync("Tab");

        await Page.GetByLabel("Confirm Password").FillAsync("pppppppp");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign Up" }).ClickAsync();
        
        var successMessage =  Page.GetByText("Registration successful. Please check your email to verify your account.");
        
        await Expect(successMessage).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 30000 });
        
    }
}