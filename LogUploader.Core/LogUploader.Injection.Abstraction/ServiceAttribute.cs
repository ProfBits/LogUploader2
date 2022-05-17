namespace LogUploader.Injection;

[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class ServiceAttribute : Attribute
{
    readonly ServiceType serviceType;

    public ServiceAttribute(ServiceType positionalString)
    {
        this.serviceType = positionalString;
    }

    public ServiceType ServiceType
    {
        get { return serviceType; }
    }
}
