using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderPatternDemo
{
    // 1. PRODUCT: Клас Документ
    // Це складний об'єкт, який ми будемо конструювати частинами.
    public class Document
    {
        private List<string> _content = new List<string>();
        private List<string> _footnotes = new List<string>();

        public void AddPart(string part)
        {
            _content.Add(part);
        }

        public void AddFootnote(string note)
        {
            _footnotes.Add(note);
        }

        public string GetContent()
        {
            StringBuilder sb = new StringBuilder();

            // Додаємо основний контент
            foreach (var part in _content)
            {
                sb.AppendLine(part);
            }

            // Якщо є виноски, додаємо їх в кінці документа
            if (_footnotes.Count > 0)
            {
                sb.AppendLine("\n--- ВИНОСКИ ---");
                for (int i = 0; i < _footnotes.Count; i++)
                {
                    sb.AppendLine($"[{i + 1}] {_footnotes[i]}");
                }
            }

            return sb.ToString();
        }
    }

    // 2. BUILDER: Інтерфейс будівельника
    // Визначає методи для додавання різних частин документа.
    public interface IDocumentBuilder
    {
        void AddHeading(string text);
        void AddSection(string text);
        void AddFootnote(string text);
        Document GetDocument();
    }

    // 3. CONCRETE BUILDER: Конкретний будівельник
    // Реалізує кроки для створення, наприклад, HTML-подібного звіту.
    // Можна створити інший будівельник (наприклад, MarkdownBuilder), не змінюючи код клієнта.
    public class HtmlDocumentBuilder : IDocumentBuilder
    {
        private Document _document = new Document();

        public void AddHeading(string text)
        {
            // Форматуємо як заголовок HTML
            _document.AddPart($"<h1>{text}</h1>");
        }

        public void AddSection(string text)
        {
            // Форматуємо як параграф
            _document.AddPart($"<p>{text}</p>");
        }

        public void AddFootnote(string text)
        {
            // Додаємо виноску (логіка зберігання виносок реалізована в продукті)
            _document.AddPart($"<sup><small>[See footnote]</small></sup>");
            _document.AddFootnote($"<small>{text}</small>");
        }

        public Document GetDocument()
        {
            Document result = _document;
            // Скидаємо об'єкт для можливості створення нового
            _document = new Document();
            return result;
        }
    }

    // 4. DIRECTOR: Директор (опціонально)
    // Відповідає за виконання кроків будівництва у певному порядку.
    public class DocumentDirector
    {
        public void ConstructTechnicalReport(IDocumentBuilder builder)
        {
            builder.AddHeading("Технічний звіт");
            builder.AddSection("Це вступна секція технічного документа.");
            builder.AddFootnote("Дані взяті з відкритих джерел.");
            builder.AddSection("Опис основної архітектури системи.");
            builder.AddHeading("Висновок");
            builder.AddSection("Система працює стабільно.");
        }
    }

    // Клієнтський код
    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо будівельника
            IDocumentBuilder builder = new HtmlDocumentBuilder();

            // Створюємо директора (або можна керувати будівельником напряму)
            DocumentDirector director = new DocumentDirector();

            Console.WriteLine("Generating Document...");

            // Директор керує процесом
            director.ConstructTechnicalReport(builder);

            // Отримуємо результат
            Document doc = builder.GetDocument();

            // Виводимо результат
            Console.WriteLine(doc.GetContent());

            /* * Приклад ручного керування (без Директора), 
             * якщо потрібна кастомна структура:
             */
            Console.WriteLine("\nGenerating Custom Document...");
            builder.AddHeading("Мій власний документ");
            builder.AddSection("Довільний текст секції.");
            builder.AddFootnote("Примітка автора.");

            Document customDoc = builder.GetDocument();
            Console.WriteLine(customDoc.GetContent());

            Console.ReadKey();
        }
    }
}