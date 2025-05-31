export interface UrlRequest {
  originalURL: string;
}

export interface UpdateUrlRequest {
  shortCode: string;
  newOriginalURL: string;
}