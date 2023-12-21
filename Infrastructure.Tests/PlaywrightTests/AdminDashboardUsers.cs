using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Infrastructure.Tests.PlaywrightTests;

[TestFixture]
public class AdminDashboardUsers : PageTest
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
    public async Task UsersDetails()
    {
        
        await AdminLoginHelper.AdminLoginAsync(Page);
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Users" }).ClickAsync();
        
        var userPage =  Page.GetByRole(AriaRole.Heading, new() { Name = "All Users" });
        
        await Expect(userPage).ToBeVisibleAsync();
        
        await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^PlayWrightDetailsUpdateDelete$") }).GetByRole(AriaRole.Button).First.ClickAsync();
        
        var successFetchUser = Page.GetByText("Successfully fetched");
        
        await Expect(successFetchUser).ToBeVisibleAsync();
        
        var userName = Page.GetByRole(AriaRole.Heading, new() { Name = "PlayWright" });
        await Expect(userName).ToBeVisibleAsync();

        var userEmail = Page.GetByText("playwright@example.com");
        await Expect(userEmail).ToBeVisibleAsync();

        var userAvatar = Page.Locator("ion-avatar");
        await Expect(userAvatar).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/update-user"));

        var backToUsersList = Page.GetByRole(AriaRole.Button, new() { Name = "Back to User List" }).ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/users"));

    }

    [Test]
    public async Task UpdateUser()
    {

        await AdminLoginHelper.AdminLoginAsync(Page);
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Users" }).ClickAsync();
        
        var updateUser = Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^PlayWrightDetailsUpdateDelete$") }).GetByRole(AriaRole.Button).Nth(1);
        await Expect(updateUser).ToBeEnabledAsync();
        
        await updateUser.ClickAsync();
        
        await Page.WaitForSelectorAsync("input[placeholder='Enter full name']");
        
        await Page.FillAsync("input[placeholder='Enter full name']", "PlayWrightTest");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Update" }).ClickAsync();
        
        var updateMessage = Page.GetByText("Successfully updated");
        await Expect(updateMessage).ToBeVisibleAsync();
        
    }

    /*[Test]
    public async Task DeleteUser()
    {
        //Helper.CreateAndDeleteAdminUser(true, false);

        await AdminLoginHelper.AdminLoginAsync(Page);
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Users" }).ClickAsync();
        
        void page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.DismissAsync(); // Asynchronously dismiss the dialog
        }
        
        Page.Dialog += page_Dialog_EventHandler;
        
        await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^PlayWrightDetailsUpdateDelete$") }).GetByRole(AriaRole.Button).Nth(2).ClickAsync();

        Page.Dialog -= page_Dialog_EventHandler;
        
        var deleteMessage = Page.GetByText("Successfully deleted");
        //await Expect(deleteMessage).ToBeVisibleAsync();

        //Helper.CreateAndDeleteAdminUser(false, true);

    }*/
}