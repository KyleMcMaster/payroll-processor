namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands
{
    public interface ICommand<TError> { }

    public interface ICommand<TError, TResponse> { }

    public interface ICommandSync<TError> { }

    public interface ICommandSync<TError, TResponse> { }
}
