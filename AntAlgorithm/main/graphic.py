import matplotlib.pyplot as plt

def read_data(file_path, step=1000):
    iterations = []
    current_paths = []
    best_paths = []
    best_path_probabilities = []

    with open(file_path, 'r') as file:
        for i, line in enumerate(file):
 
                parts = line.strip().split()
                if len(parts) == 4:
                    iteration = int(parts[0])
                    current_path = float(parts[1].replace(',', '.'))
                    best_path = float(parts[2].replace(',', '.'))
                    best_path_probability = float(parts[3].replace(',', '.'))

                    iterations.append(iteration)
                    current_paths.append(current_path)
                    best_paths.append(best_path)
                    best_path_probabilities.append(best_path_probability)

    return iterations, current_paths, best_paths, best_path_probabilities

def plot_paths(iterations, current_paths, best_paths, best_path_probabilities):
    plt.figure(figsize=(10, 6))
    plt.plot(iterations, current_paths, label='Лучший путь', marker='o', linestyle='None')
    plt.plot(iterations, best_paths, label='Текущий пут', marker='x', linestyle='None')
    plt.xlabel('Итерация')
    plt.ylabel('Длина пути')
    plt.title('Зависимость длины текущего и лучшего пути от итерации')
    plt.legend()
    plt.grid(True)
    plt.show()

    plt.figure(figsize=(10, 6))
    plt.plot(iterations, best_path_probabilities, label='Вероятность лучшего пути', marker='s', linestyle='None')
    plt.xlabel('Итерация')
    plt.ylabel('Вероятность')
    plt.title('Зависимость вероятности лучшего пути от итерации')
    plt.legend()
    plt.grid(True)
    plt.show()

if __name__ == "__main__":
    file_path = 'path.txt'
    iterations, current_paths, best_paths, best_path_probabilities = read_data(file_path, step=10)
    plot_paths(iterations, current_paths, best_paths, best_path_probabilities)