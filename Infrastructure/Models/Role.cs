using System.Runtime.Serialization;

namespace Infrastructure.Models;

public enum Role
{
    [EnumMember(Value = "Admin")]
    Admin,
    [EnumMember(Value = "Teacher")]
    Teacher,
    [EnumMember(Value = "Student")]
    Student
}