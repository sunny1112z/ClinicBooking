using ClinicBooking.Entities;
using Microsoft.EntityFrameworkCore;

public class AccountService
{
    private readonly ClinicBookingContext _context;

    public AccountService(ClinicBookingContext context)
    {
        _context = context;
    }

    public User? GetUserByUsername(string username)
    {
        return _context.Users
            .Include(u => u.Role) 
            .FirstOrDefault(u => u.Username == username);
    }
}
