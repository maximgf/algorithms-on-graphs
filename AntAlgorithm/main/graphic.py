import matplotlib.pyplot as plt

def read_data(file_path, step=1000):
    iterations = []
    current_paths = []
    best_paths = []

    with open(file_path, 'r') as file:
        for i, line in enumerate(file):
            if i % step == 0:  # Берем каждое 10-е значение
                parts = line.strip().split()
                if len(parts) == 3:
                    iteration = int(parts[0])
                    current_path = float(parts[1])
                    best_path = float(parts[2])

                    iterations.append(iteration)
                    current_paths.append(current_path)
                    best_paths.append(best_path)

    return iterations, current_paths, best_paths

def plot_paths(iterations, current_paths, best_paths):
    plt.figure(figsize=(10, 6))
    plt.plot(iterations, current_paths, label='Лучший', marker='o', linestyle='None')
    plt.plot(iterations, best_paths, label='Текущий путь', marker='x', linestyle='None')
    plt.xlabel('Итерация')
    plt.ylabel('Длина пути')
    plt.title('Зависимость длины текущего и лучшего пути от итерации')
    plt.legend()
    plt.grid(True)
    plt.show()

if __name__ == "__main__":
    file_path = 'path.txt'
    iterations, current_paths, best_paths = read_data(file_path, step=10)
    plot_paths(iterations, current_paths, best_paths)