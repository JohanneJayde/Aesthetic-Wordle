using Microsoft.AspNetCore.Authorization;

namespace Wordle.Api.Identity;
public static class Policies
{
    public const string AddOrDeleteWord = "EditWord";

    public static void AddOrDeleteWordPolicy(AuthorizationPolicyBuilder policy)
    {
        policy.RequireClaim(Claims.MasterOfTheUniverse, "true");
        policy.RequireAssertion(context =>
        {
            var birthdayString = context.User.Claims.FirstOrDefault(f => f.Type == Claims.BirthDate);

            if (DateTime.TryParse(birthdayString?.Value, out DateTime birthday))
            {
                return (DateTime.Now.Year - birthday.Year) switch
                {
                    > 21 => true,
                    21 => DateTime.Now.Month >= birthday.Month && DateTime.Now.Day >= birthday.Day,
                    _ => false
                };
            }
            return false;
        });
        }
 }

