using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Infrastructure.Tests.PlaywrightTests;

[TestFixture]
public class AdminDashboardTest : PageTest
{

    [Test]
    public async Task NavigateToAdminHomePageAndCheckIt()
    {
        
        Helper.CreateAndDeleteAdminUser(true, false);
        await Page.GotoAsync("http://localhost:5000/home");
        
        Page.WaitForNavigationAsync();

        await Page.Locator("ion-buttons").Filter(new() { HasText = "Login" }).ClickAsync();
        
        Page.WaitForNavigationAsync();

        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/login"));
        
        await Page.GetByLabel("Username or email address").ClickAsync();

        await Page.GetByLabel("Username or email address").FillAsync("test@admin.com");

        await Page.GetByLabel("Password").ClickAsync();

        await Page.GetByLabel("Password").FillAsync("TTTTTTTT");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));

        await Page.GetByRole(AriaRole.Link, new() { Name = "Users" }).ClickAsync();
        
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/users"));

        await Page.GetByRole(AriaRole.Link, new() { Name = "Courses" }).ClickAsync();
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/courses"));

        await Page.GetByRole(AriaRole.Link, new() { Name = "Lessons" }).ClickAsync();
        
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/course-lessons"));

        await Page.GetByRole(AriaRole.Button, new() { Name = "Users By Role" }).ClickAsync();
        
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/users/role/%20"));

        await Page.GetByRole(AriaRole.Button, new() { Name = "Profile" }).ClickAsync();
        
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/my-profile"));
        
        await Page.GetByRole(AriaRole.Img, new() { Name = "Learning Platform Logo" }).ClickAsync();
        
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));

        await Page.GetByRole(AriaRole.Link, new() { Name = "Home" }).ClickAsync();
        
        Page.WaitForNavigationAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));
        Helper.CreateAndDeleteAdminUser(false, true);

    }
}