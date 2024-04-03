using BlApi;
using BO;

namespace BlImplementation;
public class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private IBl bl = Factory.Get();

    public int Create(User boUser)
    {
        DO.User doUser = new DO.User();
        BO.Tools.CopySimilarFields(boUser, doUser);
        doUser = BO.Tools.UpdateEntity(doUser, "UserPermission", (DO.UserPermission)boUser.UserPermission);
        try
        {
            if (BO.Tools.ValidatePositiveNumber<int>(boUser.Id)
                && BO.Tools.ValidateEmailAddress(boUser.Email)
               )
            {
                int idUser = _dal.User.Create(doUser);
                return idUser;
            }
            return 0;
        }
        catch (DO.Exceptions.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlAlreadyExistsException($"User with ID={boUser.Id} already exists", ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.User.Delete(id);
        }
        catch (DO.Exceptions.DalDeletionImpossibleException ex)
        {
            throw new BO.Exceptions.BlDeletionImpossibleException("User not found", ex);
        }
        catch (DO.Exceptions.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"User with id={id} does not exist", ex);
        }
    }



    public User? Read(int id)
    {
        bl.Engineer.Read(id);
        DO.User doUser = _dal.User.Read(id);
        if (doUser == null) return null;

        BO.Tools.CheckActive("User", doUser);
        var boUser = new BO.User();
        BO.Tools.CopySimilarFields(doUser, boUser);
        boUser = BO.Tools.UpdateEntity(boUser, "UserPermission", (BO.UserPermission)doUser.UserPermission);
        return boUser;
    }


    public User? Login(User user)
    {
        var existingUser = Read(user.Id);
        if (existingUser == null)
        {
            // User doesn't exist, so create a new user
            if (Create(user) == null) throw new Exception("Failed to create user");
            return user;
        }

        // Check if the provided password is valid
        if (!IsPasswordValid(existingUser, user.Password))
        {
            throw new BO.Exceptions.BlInvalidCredentialsException("Invalid password");
        }

        // If passwords match, return the user
        return existingUser;
    }
    public bool IsPasswordValid(User user, string password)
    {
        // Validate the provided password against the given password
        return Equals(user.Password, password);
    }

    public void Reset()
    {
        _dal.User.Reset();
    }

}