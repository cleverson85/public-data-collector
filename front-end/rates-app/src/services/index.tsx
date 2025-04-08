import axios from 'axios';
import { config } from '../../config';

export const CurrencyRateApi = axios.create({
  baseURL: config.apiUrl,
});
