namespace Entities.Exceptions
{
    public sealed class NullParametersBadRequestException:BadRequestException
    {
        public NullParametersBadRequestException(string parameter)
            :base($"Prameter {parameter} is null")
        {

        }
    }
}
