<template>
    <div class="flex flex-col items-center mb-4">
      <h1 class="text-2xl font-bold mb-4">Currency Quotation</h1>
      <div class="flex gap-4 mb-4">
        <input
          v-model="filter.code"
          placeholder="Filter by code currency"
          class="border p-2 rounded"
        />
        <input
          type="date"
          v-model="filter.date"
          class="border p-2 rounded"
        />
        <button @click="onSearch" class="bg-blue-600 text-white px-4 py-2 rounded cursor-pointer hover:opacity-50 w-[100px]">Search</button>
      </div>
    </div>

    <table class="w-[80%] mx-auto">
      <thead>
        <tr class="bg-gray-300">
          <th class="border p-2">Target Currency</th>
          <th class="border p-2">Code</th>
          <th class="border p-2">Quotation</th>
          <th class="border p-2">Date</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="rate in rates" :key="rate.id">
          <td class="border p-2">
            <span class="flex items-center justify-center">{{ rate.targetCurrency }}</span>
          </td>
          <td class="border p-2">
            <span class="flex items-center justify-center">{{ rate.code }}</span>
          </td>
          <td class="border p-2">
            <span class="flex items-center justify-center">{{ rate.rate.toFixed(2) }}</span>
          </td>
          <td class="border p-2">
            <span class="flex items-center justify-center">{{ formatDate(rate.date)  }}</span>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="flex justify-center mt-4">
      <div>
        <button
          :disabled="page === 1"
          @click="goToPreviousPage"
          class="bg-gray-300 px-4 py-2 rounded disabled:opacity-50 cursor-pointer"
        >
          ⬅ Previous
        </button>

        <span class="mx-4">
          Page {{ page }} of {{ totalPages }}
        </span>

        <button
          :disabled="page === totalPages"
          @click="goToNextPage"
          class="bg-gray-300 px-4 py-2 rounded disabled:opacity-50 cursor-pointer"
        >
          Next ➡
        </button>
      </div>
    </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, computed } from 'vue'
  import { CurrencyRateApi } from "../services";
  
  interface CurrencyRate {
    id: string
    targetCurrency: string
    code: string
    rate: number
    date: string
  }

  const rates = ref<CurrencyRate[]>([])
  const filter = ref({ code: '', date: '' })
  const page = ref(1)
  const pageSize = 15
  const total = ref(0)

  const totalPages = computed(() =>
    Math.ceil(total.value / pageSize)
  )

  const formatDate = (dateStr: string) => {
    const date = new Date(dateStr)
    return date.toLocaleDateString()
  }

  const fetchRates = async () => {
    const params: any = {
      page: page.value,
      pageSize
    }

    if (filter.value.code) 
      params.code = filter.value.code

    if (filter.value.date) 
      params.date = filter.value.date

    const response = await CurrencyRateApi.get('api/v1/rates/filter', { params })

    rates.value = response.data.currencyRates
    total.value = response.data.total
  }

  const goToNextPage = () => {
    if (page.value < totalPages.value) {
      page.value++
      fetchRates()
    }
  }

  const goToPreviousPage = () => {
    if (page.value > 1) {
      page.value--
      fetchRates()
    }
  }

  const onSearch = () => {
    page.value = 1
    fetchRates()
  }

  onMounted(fetchRates)
</script>
