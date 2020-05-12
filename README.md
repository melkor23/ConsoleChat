# ConsoleCHAT :shrug::speech_balloon::ok_person:
  

Implementación tanto del servidor como cliente de un chat utilizando websockets con C# y.Net Core 3.1

## Librerias utilizadas 
 **Fleck** para la parte de servidor de Websockets  https://github.com/statianzo/Fleck

**WebSocketSharp** para la parte de cliente de los Websockets  https://github.com/sta/websocket-sharp

**Dependency injection** para la inyeccion de dependencias  https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/

**Newsoft.Json** para el tratamiento de Json  https://github.com/JamesNK/Newtonsoft.Json



## Utilización 
Se debe pasar como parametro el numero del puerto:

  **ConsoleChat.exe** *puerto* 

Si no hay ningun servidor escuchando en ese puerto se inicia en modo servidor. Si el puerto esta disponible se pone en modo Cliente y se conecta.

En cualquier momento se puede utilizar el comando "salir" para finalizar el websocket.


## Explicación de la estructura y de las decisiones de diseño 

La aplicación se ha dividido en servicios según en que rol en el que se inicia programa: servidor o cliente.
Cargamos los dos servicios mediante el "Dependency injection" al inicio del programa como Singleton en un contenedor IoC y dependiendo del rol, utilizamos un servicio u otro.
Se inyecta en los servicios (Cliente y servidor) la interfaz de gestion de la entrada-salida y la gestión de los strings de textos que se muestran por pantalla.

A ambos servicios le inyectamos la interfaz "ILecturaEscritura", en la que delegamos la entrada-salida por pantalla
Tambien inyectamos los ITextosConsola que son los textos que se mostraran por pantalla, almacenados en el archivo textos.json

El mensaje entre cliente-servidor se envia mediante un objeto en formato Json, separando el usuario y el mensaje. Se ha creado una clase sencilla Mensaje.cs para la estructura de los mensajes a enviar.
Cada vez que hay una conexion, el servidor se guarda en un List<WebSocket> los datos de la conexion y cada vez que llega un mensaje el servidor lo distribuye por las conexiones que estan activas, menos por la que se envio el propio mensaje. 
Al desconectarse un cliente se borra de la lista, al informar al servidor. Todos estos eventos se asignan al crear el socket tanto en modo servidor como en modo cliente
