using ClinicBooking.Entities;
using ClinicBooking_Data.Repositories.Interfaces;

public class AccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }


    public AccountService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }
    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddUserAsync(user);
    }
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task CreateUserAsync(User user)
    {
        await _userRepository.AddUserAsync(user);
    }

    public async Task<User?> RegisterAsync(string fullname, string email, string phone, string username, string password)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists.");
        }

        //  Gọi Repo để lấy Role
        var userRole = await _userRepository.GetRoleByIdAsync(2);
        if (userRole == null)
        {
            throw new Exception("Role not found.");
        }

        var newUser = new User
        {
            FullName = fullname,
            Email = email ?? throw new Exception("Email cannot be null"),
            Phone = phone ?? throw new Exception("Phone cannot be null"),
            Username = username ?? throw new Exception("Username cannot be null"),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RoleId = userRole.RoleId
        };

       


        await _userRepository.AddUserAsync(newUser);
        return newUser;
    }

}
