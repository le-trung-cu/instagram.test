export const apiStatus = {
  'Idle': Symbol('IDLE'),
  'Pending': Symbol('PENDING'),
  'Success': Symbol('SUCCESS'),
  'Error': Symbol('ERROR')
}
export const API_URL_BASE = 'https://localhost:7299/api';
export const API_REFRESH_TOKEN_URL = `${API_URL_BASE}/token/refresh`;
export const ACCESS_TOKEN = 'ACCESS_TOKEN';
export const REFRESH_TOKEN = 'REFRESH_TOKEN';
