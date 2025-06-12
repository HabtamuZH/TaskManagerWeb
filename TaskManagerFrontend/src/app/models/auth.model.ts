export interface User {
  id: number;
  username: string;
  email: string;
  role: string;
  createdAt: Date;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface LoginRequest {
  username?: string;
  password?: string;
}

export interface RegisterRequest {
  username?: string;
  email?: string;
  password?: string;
  role?: string; // Optional for user registration, backend defaults to 'User'
}


