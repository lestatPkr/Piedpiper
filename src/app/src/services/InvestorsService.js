import axios from 'axios'

const apiClient = axios.create({
  baseURL: `http://localhost:50094`,
  withCredentials: false, // This is the default
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  timeout: 10000
})

export default {
  getDashboard(id) {
    return apiClient.get(`/investors/getinvestor?investorId=${id}`)
  }
  
}