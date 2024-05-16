using HardwareOnlineStore.DataAccess.Providers.Relational.Abstractions;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Order;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Product;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.User;

namespace HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer;

public sealed class SqlServerRepository(ConnectionParameters connectionParameters)
{
    private readonly Lazy<UserRepository> _userRepository = new Lazy<UserRepository>(() => new UserRepository(new SqlServerProvider<UserEntity>(connectionParameters)));
    private readonly Lazy<ProductRepository> _productRepository = new Lazy<ProductRepository>(() => new ProductRepository(new SqlServerProvider<ProductEntity>(connectionParameters)));
    private readonly Lazy<OrderRepository> _orderRepository = new Lazy<OrderRepository>(() => new OrderRepository(new SqlServerProvider<OrderEntity>(connectionParameters)));

    public UserRepository UserRepository => _userRepository.Value;

    public ProductRepository ProductRepository => _productRepository.Value;

    public OrderRepository Order => _orderRepository.Value;
}