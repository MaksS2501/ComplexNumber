using System;
using System.Threading;
using System.Threading.Tasks;

public class ComplexNumber
{
    // Поля для алгебраической формы
    private double real; // Действительная часть комплексного числа
    private double imaginary; // Мнимая часть комплексного числа

    // Поля для тригонометрической формы
    private double magnitude; // Модуль комплексного числа
    private double argument; // Аргумент комплексного числа

    // Конструктор по умолчанию
    public ComplexNumber()
    {
        real = 0;
        imaginary = 0;
        updateTrigonometricForm();
    }

    // Конструктор с параметрами для алгебраической формы
    public ComplexNumber(double real, double imaginary)
    {
        this.real = real;
        this.imaginary = imaginary;
        updateTrigonometricForm();
    }

    // Конструктор с параметрами для тригонометрической формы
    public ComplexNumber(double magnitude, double argument, bool isTrigonometric)
    {
        if (isTrigonometric)
        {
            this.magnitude = magnitude;
            this.argument = argument;
            updateAlgebraicForm();
        }
        else
        {
            throw new ArgumentException("Invalid constructor parameters");
        }
    }

    // Деструктор
    ~ComplexNumber()
    {
        // Освобождение ресурсов, если необходимо
    }

    // Метод для обновления тригонометрической формы
    private void updateTrigonometricForm()
    {
        magnitude = Math.Sqrt(real * real + imaginary * imaginary);
        argument = Math.Atan2(imaginary, real);
    }

    // Метод для обновления алгебраической формы
    private void updateAlgebraicForm()
    {
        real = magnitude * Math.Cos(argument);
        imaginary = magnitude * Math.Sin(argument);
    }

    // Перегрузка оператора сложения
    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.real + b.real, a.imaginary + b.imaginary);
    }

    // Перегрузка оператора вычитания
    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.real - b.real, a.imaginary - b.imaginary);
    }

    // Перегрузка оператора умножения
    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
    {
        double real = a.real * b.real - a.imaginary * b.imaginary;
        double imaginary = a.real * b.imaginary + a.imaginary * b.real;
        return new ComplexNumber(real, imaginary);
    }

    // Перегрузка оператора деления
    public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
    {
        double denominator = b.real * b.real + b.imaginary * b.imaginary;
        double real = (a.real * b.real + a.imaginary * b.imaginary) / denominator;
        double imaginary = (a.imaginary * b.real - a.real * b.imaginary) / denominator;
        return new ComplexNumber(real, imaginary);
    }

    // Перегрузка оператора равенства
    public static bool operator ==(ComplexNumber a, ComplexNumber b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return false;
        }

        return a.real == b.real && a.imaginary == b.imaginary;
    }

    // Перегрузка оператора неравенства
    public static bool operator !=(ComplexNumber a, ComplexNumber b)
    {
        return !(a == b);
    }

    // Переопределение метода Equals
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null) || GetType() != obj.GetType())
        {
            return false;
        }

        ComplexNumber other = (ComplexNumber)obj;
        return real == other.real && imaginary == other.imaginary;
    }

    // Переопределение метода GetHashCode
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + real.GetHashCode();
            hash = hash * 23 + imaginary.GetHashCode();
            return hash;
        }
    }

    // Переопределение метода ToString
    public override string ToString()
    {
        return $"{real} + {imaginary}i";
    }

    // Статический метод для получения комплексного числа из строки
    public static ComplexNumber parse(string s)
    {
        string[] parts = s.Split(new[] { '+', 'i' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            throw new FormatException("Invalid format for complex number. Expected format: 'a + bi'");
        }
        double real = double.Parse(parts[0].Trim());
        double imaginary = double.Parse(parts[1].Trim());
        return new ComplexNumber(real, imaginary);
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        double real1 = getDoubleInput("Введите действительную часть первого комплексного числа:");
        double imaginary1 = getDoubleInput("Введите мнимую часть первого комплексного числа:");

        double real2 = getDoubleInput("Введите действительную часть второго комплексного числа:");
        double imaginary2 = getDoubleInput("Введите мнимую часть второго комплексного числа:");

        ComplexNumber a = new ComplexNumber(real1, imaginary1);
        ComplexNumber b = new ComplexNumber(real2, imaginary2);

        ComplexNumber sum = a + b;
        ComplexNumber difference = a - b;
        ComplexNumber product = a * b;
        ComplexNumber quotient = a / b;

        Console.WriteLine("Sum: " + sum);
        Console.WriteLine("Difference: " + difference);
        Console.WriteLine("Product: " + product);
        Console.WriteLine("Quotient: " + quotient);

        Console.WriteLine("Введите комплексное число в формате 'a + bi':");
        string input = Console.ReadLine();
        try
        {
            ComplexNumber parsed = ComplexNumber.parse(input);
            Console.WriteLine("Parsed: " + parsed);
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }

    // Метод для получения числового ввода от пользователя
    static double getDoubleInput(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = readLineWithTimeout(TimeSpan.FromSeconds(7));
            if (input == null)
            {
                Console.WriteLine("Время ожидания истекло. Пожалуйста, попробуйте снова.");
                continue;
            }
            if (double.TryParse(input, out double result))
            {
                return result;
            }
            Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
        }
    }

    // Метод для чтения строки с таймаутом
    static string readLineWithTimeout(TimeSpan timeout)
    {
        var task = Task.Run(() => Console.ReadLine());
        if (task.Wait(timeout))
        {
            return task.Result;
        }
        return null;
    }
}
