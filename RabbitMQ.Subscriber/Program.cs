using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


/*
 Önemli :  Diyelim 2 subscriber var, kaç mesaj yollanabilir belirleyeiliyoruz.
ya subs1 e 1, subs2ye 1 ya da 5er desek subs1e 5 subs2ye 5. 
Eğer mesjaalrın işlenmesi uzun sürüyosa ve 7 mesaj varsa 5er 5er yollarken, subs2 2 taneyi işlediğinde subs1 hala 3 tane daha işlemeli, subs2 onun tirmesini bekler. Bu nedenle kaçar mesay alabileceğini/ yollanabileceğini seçerken buna dikkat etmek gerek.
Mesaj işlemek uzun sürüyosa azar azar, kısa sürüyosa çok sayıda yollanması uygun olabilir!
 
 */

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