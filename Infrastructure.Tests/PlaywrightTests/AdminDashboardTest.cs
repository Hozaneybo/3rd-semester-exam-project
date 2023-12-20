using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Infrastructure.Tests.PlaywrightTests;

[TestFixture]
public class AdminDashboardTest : PageTest
{
    
    /*
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
    public async Task NavigateToAdminHomePageAndCheckIt()
    {
        
        await AdminLoginHelper.AdminLoginAsync(Page);
        
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

    }*/
}