namespace SharedKernel.Abstractions;

public interface IEntityId<T>
{
    #region Fields, Properties and Indexers
    
    public T Id { get; set; }
    
    #endregion
}