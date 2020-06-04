namespace MyMediaCollection.ViewModels
{
    /// <summary>
    /// An object that implements this interface is validatable. This is like the IValidatableObject interface in System.ComponentModel.DataAnnotations
    /// </summary>
    public interface IValidatable
    {
        void Validate(string memberName, object value);
    }
}