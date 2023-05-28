# Утилиты для работы с чертежами в Space Engineers

Подумав, что работать с текстовым окном может оказаться не для всех удобным, набросал программу с графическим интерфейсом. В остальном функционал абсолютно идентичен [версии для PowerShell](https://github.com/Homatoz/SE_Blueprint_Utils)

## Sandbox2Blueprints

Выдирает из файла мира все находящиеся в нем объекты и превращает их в файлы чертежа.

### Подготовка к запуску

[Скачать последнюю версию программы](https://github.com/Homatoz/SE_Blueprint_Utils_CSharp/releases/latest)

[Скачать .NET Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

### Запуск программы

Для запуска программы распакуйте скачанный архив и откройте файл

```text
Sandbox2Blueprints.exe
```

### Меню

При запуске программы появится окно со следующим меню.

![image](https://lh3.googleusercontent.com/GR85d9cq_PS2ZwkG6XZv_M0WMoze1LJrOIiw4LAvgzf9DwZASAwYcgs0ZsP3lymYTttXLIuZeztIc4o6MPx92NIrWPYl-8H7vSwxG8OGXwq0h9KxaJADqcSBVxKDaK7vQnpDspnVhLM2WF8y7K0wTg7M5yH0GtOb3H_kFAWxrxevHaS7xE7Ceib7yl-6vnfAIyOoPci6Zr2oNfDc9u-4_-XSi5kzJDlUNPuLIzjQpRWYpJQEM_3A1ciwH2bX5qiWnl41JSSKBJDiaiIcYmJDT1fw2_Pb9WLQkXOdKm-P2XcykXlbJPK6Wuk1cvjsPKC7YS4KlaFyn3SyYBSgLbw26BF9dhF5OB76M6CQWrt64t9uoxSbYHhd5WWZrxifvNwjwq4LwT7dGii4sxAzf26cQ8VCYdr_PLfBPGGeVJFbWn1qrrwpjW1PHN0EBKAl2RFnes5Dqe8SaGeXPeT0lNhC9jvk4fof8dXU-lUJUqC-hjXRWxH832pgrH42NALNC8pWIHIP3oVDEtjW_yWvUjf-sr4ytNfii8IxE3OIXSMGnsceKrE5cOkmoCD-ks6wjb0kyNV_fQYYMAPPJIolvfmmmz2n0Vkfve4g8NR2_9I4J9_ya9lg0L0ZbIfGmrkfW8V9yvODWhBCTkDEQ2zdrBmHNM29QqUj-Bt6MCKdRgDyOYBWKgU_prEuUI4xt5beBZ9g7AiS_B3INcwZbGhHuzqe40ALG5XyxfzkHwhP9IsXmt6x1dhprMULoe5ytFVVhAk1oAEy7VX4oBDqXYq7BxMT5GBcEIfKM6oPZdnMgyzX9XfG_YH1he7vrXGNFGotx3_EKToPbC1hs-3YNVMEPgeLpRAn0VJDM7ycLVALLaNtDjVfZefCFIiDsKKYSQxN81vPkisJhojHEOS7Skn64G8Xyy9dcAgJ_XJWpHZO7MVb1ssd=w386-h200-s-no?authuser=0)

Верхняя половина окна содержит список настроек.

В нижней половине находятся кнопки, нажатием на которые можно выбрать способ обработки файла мира.

#### Обработать файл мира в папке с программой

Для возможности выбора данного пункта требуется, чтобы файл `SANDBOX_0_0_0_.sbs` был размещен в папке с программой. Результат также будет размещен в этой папке. Если файл отсутствует, то кнопка будет неактивна.

#### Выбрать файл мира для обработки

При выборе данного пункта появятся окна, в которых надо будет выбрать файл `SANDBOX_0_0_0_.sbs`, который планируется превратить в чертежи, и папку, в которую эти чертежи будут сохранены. По умолчанию для сохранения предлагается папка, в которой игра хранит локальные чертежи - `%AppData%\SpaceEngineers\Blueprints\local`.

#### Выбрать папку для обработки файлов мира во вложенных папках

При выборе данного пункта появятся окна, в которых надо будет выбрать папку, где хранятся файлы миров, которые планируется превратить в чертежи, и папку, в которую эти чертежи будут сохранены. По умолчанию для сохранения предлагается папка, в которой игра хранит локальные чертежи - `%AppData%\SpaceEngineers\Blueprints\local`. Чертежи будут размещаться с сохранением структуры исходной папки.

> **Совет:** Данным способом можно получить все чертежи из сценариев и миров игры. Для этого в качестве папки, в которой хранятся файлы миров, указать папку с игрой.

### Результаты выполнения

После завершения обработки в папке, выбранной для сохранения, появятся множество подпапок с чертежами со следующим именованием

```text
XXXXpX_Name Multi
```

`XXXX` - это порядковый номер расположения объекта в файле мира.

`pX` - добавляется только при наличии у объекта проекторов с чертежами. При наличии вложенных проекторов это значение будет указано в имени папки несколько раз.

`Name` - название объекта, взятое из параметра DisplayName.

`Multi` - добавляется только для объединенных объектов.

Также в случае если были сформированы объединенные объекты, в папке будет находиться файл `MultiList.txt`, в котором будет указано название чертежа с мультиобъектами и номера одиночных объектов, из которых он состоит.

```text
0063_PV-4-SV3 Multi - 0060 0061 0062 0063 0064
```
