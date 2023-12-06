# Веб-сайт для обучения работе в командной строке операционной системы Linux с возможностью  интерактивного  выполнения команд
![penguin](https://github.com/D1qm0nd/BashLearningSystem/blob/main/WebApp.LearningSystem/wwwroot/Linux-Logo.png)
- Диаграмма взаимодействий
``` mermaid
sequenceDiagram
    actor Клиент
    Клиент ->> Веб-сайт: Открытие страницы сайта
    Веб-сайт ->> Слой данных : Запрос тем
    Слой данных ->> Слой данных: Загрузка тем
    Слой данных ->> Веб-сайт: Отправка тем для отображения
    Веб-сайт ->> Веб-сайт: Отображение тем
    Клиент -->> Веб-сайт : Регистрация на сайте
    Веб-сайт ->> Слой данных : Регистрация пользователя
    Слой данных ->> Слой данных: Добавление нового пользователя
    Клиент -->> Веб-сайт : Аутентификация на сайте
    Веб-сайт ->> Слой данных : Запрос данных о пользователе
    Слой данных ->> Веб-сайт : Отправка данных о пользователе
    Веб-сайт ->> Веб-сайт : Авторизация пользователя
    Клиент -->> Веб-сайт : Запрос на интерактивное выполение команды
    Веб-сайт ->> Распределитель контейнеров: Отправка команды
    Распределитель контейнеров ->> Распределитель контейнеров: выделение, запуск контенера
    Распределитель контейнеров ->> Контейнер : Отправка команды
    Контейнер ->> Контейнер : Выполение команды
    Контейнер ->> Распределитель контейнеров : Результат выполнения команды
    Распределитель контейнеров ->> Веб-сайт : Результат выполнения команд
    Распределитель контейнеров ->> Распределитель контейнеров : освобождение контейнера
    Веб-сайт ->> Веб-сайт : Отображение результата выполнения
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
