# Веб-сайт для обучения работе в командной строке операционной системы Linux с возможностью  интерактивного  выполнения команд
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
