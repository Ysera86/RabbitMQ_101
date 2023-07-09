using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Port = 5672;
factory.HostName = "localhost";
factory.UserName = "guest";
factory.Password = "guest";

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//channel.QueueDeclare("queue-name", true, false, false);
// 1 - bu satırı silersek ve daha önce publisher bu isimde kuyruk oluşturmamış olursak hata alırız.
// 2 - bu satırı bırakırız ve publisher yine daha öncesinde bu isimde kuyruk oluşturmamış olursa subscriber oluşturur.
// publisher kesin oluşturduysa, eminsek silebiliriz.
// zaten varsa kuyruk burada da olması hata vermez. Sadece aynı isimde kuyruk oluşturuyosak tamamen aynı parametrelerle oluşturduğumuzdan  emin olmalıyız.


var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume("queue-name", true, consumer);
/* autoAck : true > RabbitMQ subscribera bi mesaj gönderdiğinde, bu mesaj doğru da işlense yanlış da işlense RabbitMQ bu mesajı kuyruktan siler
 autoAck : false ise sen bunu direk silme, mesaj doğru işlenirse ben sana silmen için haber vericem demiş oluyoruz. > gerçek dünyada */


consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());

    Console.WriteLine($"Gelen mesaj : {message}");

};

Console.ReadLine();