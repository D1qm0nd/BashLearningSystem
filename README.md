# Веб-сайт для обучения работе в командной строке операционной системы Linux с возможностью  интерактивного  выполнения команд
![penguin](https://github.com/D1qm0nd/BashLearningSystem/blob/main/WebApp.LearningSystem/wwwroot/Linux-Logo.png)
- Диаграмма взаимодействий
``` mermaid
sequenceDiagram
    Actor User
    User ->> WebSite: Открытие страницы сайта
    WebSite ->> DataLayer : Запрос данных для отображения
    DataLayer ->> WebSite: Отправка данных для отображения
    WebSite ->> WebSite: Отображение данных
    User -->> WebSite : Регистрация на сайте
    WebSite ->> DataLayer : Добавление пользователя
    User -->> WebSite : Авторизация на сайте
    WebSite ->> DataLayer : Запрос данных о пользователе
    DataLayer ->> WebSite : Отправка данных о пользователе
    WebSite ->> WebSite : Расширение функционала пользователя
    User -->> WebSite : Интерактивное прохождение практики
    WebSite ->> Executor: Отправка команды
    Executor ->> Executor: Выполнение команды
    Executor ->> WebSite : Результат выполнения команды
    WebSite ->> WebSite : Отображение результата выполненияЪ
    User -->> WebSite: Посмотреть прогресс
    WebSite ->> DataLayer : Получить прогресс пользователя
    DataLayer ->> WebSite : Отправка данных

```

- Диаграмма логической модели базы данных
``` mermaid
classDiagram
direction RL

class Accounts {
   text Login
   text Password
   text Surname
   text Name
   text MiddleName
   bytea Image
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid AccountId
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

class Exercises {
   text Name
   text Text
   uuid ThemeId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid ExerciseId
}

class History {
   uuid AccountId
   uuid ExerciseId
   text status
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid HistoryId
}
class Quests {
   text Text
   text Answer
   uuid ExerciseId
   uuid CommandId
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid QuestionId
}
class Themes {
   text Name
   timestamp with time zone CreatedUTC
   timestamp with time zone UpdatedUTC
   uuid ThemeId
}

Attributes  -->  Commands : CommandId
Commands  -->  Themes : ThemeId
Exercises  -->  Themes : ThemeId
History  -->  Accounts : AccountId
History  -->  Exercises : ExerciseId
Quests  -->  Commands : CommandId
Quests  -->  Exercises : ExerciseId

```
