export interface Task {
  id: number;
  title: string;
  description: string | null;
  status: string;
  priority: string;
  createdAt: Date;
  updatedAt: Date;
  dueDate: Date | null;
  assignedToId: number | null;
  assignedToUsername: string | null;
  createdById: number;
  createdByUsername: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string | null;
  priority?: string;
  dueDate?: Date | null;
  assignedToId?: number | null;
}

export interface UpdateTaskRequest {
  title?: string;
  description?: string | null;
  status?: string;
  priority?: string;
  dueDate?: Date | null;
  assignedToId?: number | null;
}


