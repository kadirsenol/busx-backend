# .NET Core Proje - README

## Ön Koşullar
- .NET 8.0 SDK
- Visual Studio 2022 veya üstü (VS Code için C# eklentisi)
- (Opsiyonel) Docker ve Docker Compose

---

## 1. Visual Studio ile Çalıştırma

1. Projeyi Visual Studio’da açın.
2. `GenApi` projesini sağ tıklayın ve **Set as Startup Project** seçin.
3. `F5` veya **Start Debugging** ile çalıştırın.

---

## 2. VS Code ile Çalıştırma

1. VS Code ile proje dizinini açın.
2. Terminal açın ve `GenApi` projesinin path’ine gidin:
3. ...\busx-backend\BusX.GEN.API
4. dotnet run

## 3. Docker/Docker Compose ile Çalıştırma
1. Terminalde proje ana dizinini açın.
2. ...\busx-backend
3. docker-compose up --build -d
4. İlgili image oluşturulup container ile başlatılacaktır.

## Sqlite gömülü şekilde pushlandığı için gerek debug modda gerek docker ile çalıştırırken herhangi bir ayar yapılmasına gerek kalmaksızın db bağlantısı aktif çalışacaktır
