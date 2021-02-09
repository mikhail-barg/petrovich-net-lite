# petrovich-net-lite
Легковесный C#-порт проекта [Petrovich](https://github.com/petrovich) - склонение русских фамилий, имен и отчеств.
См. также [официальный C#-порт (NPetrovich)](https://github.com/petrovich/petrovich-net)

[![Build status](https://ci.appveyor.com/api/projects/status/0pep5lf0o67wnb1r?svg=true)](https://ci.appveyor.com/project/mikhail-barg/petrovich-net-lite)
[![NuGet](https://img.shields.io/nuget/v/NPetrovichLite.svg)](https://www.nuget.org/packages/NPetrovichLite/)

## API
Весь публичный API доступен через класс Petrovich. Функционал:

##### Склонение одного элемента ФИО:
```C#
Petrovich petrovich = new Petrovich();  //при создании загружаются правила
Console.WriteLine(petrovich.Inflect("Маша", NamePart.FirstName, Case.Dative));  //"Маше"
Console.WriteLine(petrovich.Inflect("Паша", NamePart.FirstName, Case.Dative));  //"Паше"
Console.WriteLine(petrovich.Inflect("Саша", NamePart.FirstName, Case.Dative, Gender.Female)); //"Саше"
```

##### Сконение ФИО целиком:
```C#
Petrovich.FIO a = petrovich.Inflect(new Petrovich.FIO() { lastName = "Пушкин", firstName = "Александр", midName = "Сергеевич" }, Case.Dative);
Console.WriteLine(a); //"Пушкину Александру Сергеевичу"
Petrovich.FIO b = petrovich.Inflect(new Petrovich.FIO() { lastName = "Воробей" }, Case.Dative, Gender.Female);
Console.WriteLine(b); //"Воробей"
Petrovich.FIO c = petrovich.Inflect(new Petrovich.FIO() { lastName = "Воробей", firstName = "Александр" }, Case.Dative);
Console.WriteLine(c); //"Воробью Александру"
```

##### Определение пола:
```C#
Console.WriteLine(petrovich.GetGender("Пушкин", NamePart.LastName));  //Male
Console.WriteLine(petrovich.GetGender("Пушкин", null, "Сергеевич"));  //Male
Console.WriteLine(petrovich.GetGender(new Petrovich.FIO() { lastName = "Воробей", firstName = "Александр" })); //Male
```


## Зависимости
Зависимости времени выполнения отсутствуют — никаких внешних бибилиотек!


## Падежи:

|Case|Падеж|Характеризующий вопрос|
|---|---|---|
|Nominative|Именительный|Кто? Что?|
|Genetive|Родительный|Кого? Чего?|
|Dative|Дательный|Кому? Чему?|
|Accusative|Винительный|Кого? Что?|
|Instrumental|Творительный|Кем? Чем?|
|Prepositional|Предложный|О ком? О чём?|
