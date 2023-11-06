# Веб-сайт для обучения работе в командной строке операционной системы Linux с возможностью  интерактивного  выполнения команд
![penguin](https://github.com/D1qm0nd/BashLearningSystem/blob/main/WebApp.LearningSystem/wwwroot/Linux-Logo.png)
- Диаграмма взаимодействий
``` mermaid
sequenceDiagram
    Actor User
    User ->> WebSite: Открытие страницы сайта
    WebSite ->> DataLayer : Запрос тем
    DataLayer ->> WebSite: Отправка тем для отображения
    WebSite ->> WebSite: Отображение тем
    User -->> WebSite : Регистрация на сайте
    WebSite ->> DataLayer : Регистрация пользователя
    User -->> WebSite : Аутентификация на сайте
    WebSite ->> DataLayer : Запрос данных о пользователе
    DataLayer ->> WebSite : Отправка данных о пользователе
    WebSite ->> WebSite : Авторизация пользователя
    User -->> WebSite : Запрос на интерактивное выполение команды
    WebSite ->> Executor: Отправка команды
    Executor ->> Executor: Выполнение команды
    Executor ->> WebSite : Результат выполнения команды
    WebSite ->> WebSite : Отображение результата выполнения

```

- Диаграмма логической модели базы данных
``` mermaid
classDiagram
direction BT
class Admins {
   uuid UserId
   boolean IsActual
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid AdminId
}
class Attributes {
   text Text
   text Description
   uuid CommandId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid AttributeId
}
class Commands {
   text Text
   text Description
   uuid ThemeId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid CommandId
}
class Questions {
   text Text
   uuid ThemeId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid QuestionId
}
class Reads {
   uuid ThemeId
   uuid UserId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid ReadId
}
class Themes {
   text Name
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid ThemeId
}
class Users {
   text Login
   text Password
   text Name
   text Surname
   text Middlename
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid UserId
}

Admins  -->  Users : UserId
Attributes  -->  Commands : CommandId
Commands  -->  Themes : ThemeId
Questions  -->  Themes : ThemeId
Reads  -->  Themes : ThemeId
Reads  -->  Users : UserId


```
#Отношения
``` mermaid
erDiagram 

Admins {
   uuid UserId
   boolean IsActual
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid AdminId
}

Attributes {
   text Text
   text Description
   uuid CommandId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid AttributeId
}

Commands {
   text Text
   text Description
   uuid ThemeId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid CommandId
}
Questions {
   text Text
   uuid ThemeId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid QuestionId
}
Reads {
   uuid ThemeId
   uuid UserId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid ReadId
}
Themes {
   text Name
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid ThemeId
}
Users {
   text Login
   text Password
   text Name
   text Surname
   text Middlename
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid UserId
}

Admins  }o--||  Users : UserId
Attributes  }o--||  Commands : CommandId
Commands  }o--|{  Themes : ThemeId
Questions  }o--|{  Themes : ThemeId
Reads  }o--|{  Themes : ThemeId
Reads  }o--|{ Users : UserId
```
